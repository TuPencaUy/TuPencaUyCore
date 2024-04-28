using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TuPencaUy.Platform.DAO.Models.Data
{
  public class PlatformDbContext : DbContext
  {
    public PlatformDbContext(DbContextOptions<PlatformDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("User");
      modelBuilder.Entity<Role>().ToTable("Role");
      modelBuilder.Entity<Permission>().ToTable("Permission");
    }
  }
}
