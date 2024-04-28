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
  }
}
