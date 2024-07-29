using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace eBook_Library.services.AuthService;

public class GenToken {
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
     
    public GenToken(IConfiguration configuration UserManager userManager) {
        _configuration = configuration;
        _userManager = userManager;
    }


    public string? GenerateToken(string email) {
        var secret = _configuration["JwtConfig:Secret"];
        var issuer = _configuration["JwtConfig:ValidIssuer"];
        var audience = _configuration["JwtConfig:ValidAudiences"];
        if(secret is null || issuer is null || audience is null) {
            throw new ApplicationException("Jwt is not set in the configuration");
        }
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>{ new ( ClaimTypes.Name, email) };
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };
        
        var SecurityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(SecurityToken);
        return token;
    }
}