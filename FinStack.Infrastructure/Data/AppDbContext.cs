using Microsoft.EntityFrameworkCore;
using FinStack.Domain.Entities;
using FinStack.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FinStack.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AuthUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
}
