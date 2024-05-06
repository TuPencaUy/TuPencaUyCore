using Microsoft.EntityFrameworkCore;

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
    public DbSet<Site> Sites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Rename tables removing plurality
      modelBuilder.Entity<User>().ToTable("User");
      modelBuilder.Entity<Role>().ToTable("Role");
      modelBuilder.Entity<Permission>().ToTable("Permission");
      modelBuilder.Entity<Site>().ToTable("Site");
    }
  }
}
