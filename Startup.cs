using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using eBook_Library.services.AuthService;
using eBook_Library.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public class Startup {
    public IConfiguration Configuration { get; }

    public Startup (IConfiguration configuration) {
       Configuration = configuration;
    }

   public void ConfigureServices(IServiceCollection services) {
       // add your services here
       services.AddControllers();
       
       services.AddScoped<GenToken>();
       services.AddScoped<IUserService, UserService>();
       services.AddSwaggerGen();
       services.AddDbContext<UserDbContext>(options =>
            options.UseInMemoryDatabase("UserDB"));

        // Configure Identity services
        services.AddDbContext<UserDbContext>();
        services.AddIdentity<User, IdentityRole>()
            .AddRoles<IdentityRole>();
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        // Configure jwt scheme
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>{
            var secret = Configuration["JwtConfig:Secret"];
            var issuer = Configuration["JwtConfig:ValidIssuer"];
            var audience = Configuration["JwtConfig:ValidAudiences"];

            if(secret is null || issuer is null || audience is null) {
                throw new ApplicationException("Jwt is not set in the configuration");
            }
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters() {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
            };
        });

        // Configure LibraryContext for library data
        services.AddDbContext<LibraryContext>(options =>
            options.UseInMemoryDatabase("LibraryDB"));

        // services.AddControllers();
        services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "fixed", options => {
            options.PermitLimit = 5;
            options.Window = TimeSpan.FromSeconds(10);
            options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 2;
        }));

        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1",});
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme(){
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement { 
            { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }},
            new string[] {} }
            });
        });

   }

   public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
       //add configuration/middleware here
       if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
       }
       app.UseSwaggerUI();
       app.UseRateLimiter();
       app.UseHttpsRedirection();
       app.UseAuthentication();
       app.UseRouting();
       app.UseAuthorization();

       using (var serviceScope = app.services.CreateScope()) {
        var services = serviceScope.ServiceProvider;

        // Ensure the database is created.
        var dbContext = services.GetRequiredService<AppDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(URoles.Member)) {
            await roleManager.CreateAsync(new IdentityRole(URoles.Member));
        }

        if (!await roleManager.RoleExistsAsync(URoles.Librarian)) {
            await roleManager.CreateAsync(new IdentityRole(URoles.Librarian));
        }

        if (!await roleManager.RoleExistsAsync(URoles.Administrator)) {
            await roleManager.CreateAsync(new IdentityRole(URoles.Administrator));
        }

        if (!await roleManager.RoleExistsAsync(URoles.Guest)) {
            await roleManager.CreateAsync(new IdentityRole(URoles.Guest))
        }
     }     
       app.UseEndpoints(endpoints => { endpoints.MapControllers();
       });
       app.UseSwagger();
   }
 
}
