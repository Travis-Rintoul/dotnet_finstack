using Microsoft.EntityFrameworkCore;
using FinStack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FinStack.Infrastructure.Data;

public class AuthRole : IdentityRole<Guid>
{
}

public class AppDbContext : IdentityDbContext<AuthUser, AuthRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> AppUsers { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .ToTable("AppUsers")
            .HasKey(u => u.UserGuid);

        builder.Entity<User>()
            .HasOne(u => u.AuthUser)
            .WithOne(a => a.AppUser)
            .HasForeignKey<User>(u => u.UserGuid);

        builder.Entity<AuthUser>().ToTable("AuthUsers");
        builder.Entity<IdentityRole>().ToTable("AuthRoles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AuthUserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AuthUserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AuthUserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AuthRoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AuthUserTokens");
    }
}