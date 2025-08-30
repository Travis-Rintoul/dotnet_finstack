using Microsoft.EntityFrameworkCore;
using FinStack.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FinStack.Common;

namespace FinStack.Infrastructure.Data;

public class AuthRole : IdentityRole<Guid>
{
}

public class AppDbContext : IdentityDbContext<AuthUser, AuthRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .ToTable("AppUsers")
            .HasKey(u => u.UserGuid);

        builder.Entity<AppUser>()
            .HasOne(u => u.AuthUser)
            .WithOne(a => a.AppUser)
            .HasForeignKey<AppUser>(u => u.UserGuid);

        builder.Entity<AuthUser>().ToTable("AuthUsers");
        builder.Entity<IdentityRole>().ToTable("AuthRoles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AuthUserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AuthUserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AuthUserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AuthRoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AuthUserTokens");
    }

    public async Task EnsureConnectionAsync()
    {
        var connection = Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }
    }

    public async Task EnsureConnectionClosedAsync()
    {
        var connection = Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Closed)
        {
            await connection.CloseAsync();
        }
    }
}

public static class AppDbContextExtensions
{
    public static async Task<bool> WaitForExistAsync<T>(
        this AppDbContext db,
        Func<IQueryable<T>, IQueryable<T>> queryBuilder,
        TimeSpan timeout,
        TimeSpan pollInterval,
        CancellationToken ct = default)
        where T : class
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            var query = queryBuilder(db.Set<T>().AsNoTracking());
            if (await query.AnyAsync(ct).ConfigureAwait(false))
                return true;

            await Task.Delay(pollInterval, ct).ConfigureAwait(false);
        }
        return false;
    }

    public static async Task<Option<T>> WaitForRecordAsync<T>(
        this AppDbContext db,
        Func<IQueryable<T>, IQueryable<T>> queryBuilder,
        TimeSpan timeout,
        TimeSpan pollInterval,
        CancellationToken ct = default)
        where T : class
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            var query = queryBuilder(db.Set<T>().AsNoTracking());

            var result = await query.SingleOrDefaultAsync(ct).ConfigureAwait(false);

            if (result != null)
            {
                return Option.Some(result);
            }
            await Task.Delay(pollInterval, ct).ConfigureAwait(false);
        }

        return Option.None<T>();
    }
}