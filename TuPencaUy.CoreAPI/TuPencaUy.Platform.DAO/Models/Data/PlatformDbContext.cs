using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TuPencaUy.Core.DAO;

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
    public DbSet<Event> Events { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Core.DAO.Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Rename tables removing plurality
      modelBuilder.Entity<User>().ToTable("User");
      modelBuilder.Entity<Role>().ToTable("Role");
      modelBuilder.Entity<Permission>().ToTable("Permission");
      modelBuilder.Entity<Site>().ToTable("Site");
      modelBuilder.Entity<Event>().ToTable("Event");
      modelBuilder.Entity<Sport>().ToTable("Sport");
      modelBuilder.Entity<Core.DAO.Match>().ToTable("Match");
      modelBuilder.Entity<Team>().ToTable("Team");
    }
  }
}
