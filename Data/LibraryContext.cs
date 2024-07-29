using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class User: IdentityUser { }
public class UserDbContext: IdentityDbContext<User> {

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
}

public class LibraryContext : DbContext {
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Additional configuration if needed
    }
}
