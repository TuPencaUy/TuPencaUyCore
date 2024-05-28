using Microsoft.EntityFrameworkCore;

namespace TuPencaUy.Site.DAO.Models.Data
{
  public class SiteDbContext : DbContext
  {
    public SiteDbContext(DbContextOptions<SiteDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("User");
      modelBuilder.Entity<Role>().ToTable("Role");
      modelBuilder.Entity<Permission>().ToTable("Permission");
      modelBuilder.Entity<Event>().ToTable("Event");
      modelBuilder.Entity<Sport>().ToTable("Sport");
      modelBuilder.Entity<Match>().ToTable("Match");
      modelBuilder.Entity<Team>().ToTable("Team");
    }
  }
}
