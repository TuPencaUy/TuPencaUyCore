using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Site.DAO.Models.Data;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformSiteService : ISiteService
  {
    private readonly IGenericRepository<TuPencaUy.Platform.DAO.Models.Site> _siteDAL;
    private readonly IGenericRepository<TuPencaUy.Platform.DAO.Models.User> _userDAL;
    private readonly IConfiguration _configuration;

    public PlatformSiteService(
      IGenericRepository<TuPencaUy.Platform.DAO.Models.Site> siteDAL,
      IGenericRepository<TuPencaUy.Platform.DAO.Models.User> userDAL,
      IConfiguration configuration)
    {
      _siteDAL = siteDAL;
      _userDAL = userDAL;
      _configuration = configuration;
    }

    public SiteDTO GetSiteByDomain(string domain)
    {
      return _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>>
          { x => x.Domain == domain })
        .Select(x => new SiteDTO
        {
          Id = x.Id,
          Name = x.Name,
          Domain = domain,
          ConnectionString = x.ConnectionString,
          Color = x.Color,
          AccessType = x.AccessType,
        })
        .FirstOrDefault() ?? throw new SiteNotFoundException($"No site was found with domain {domain}");
    }

    public List<UserDTO> GetUsers(string domain)
    {
      var site = _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>>
          { x => x.Domain == domain })
        .FirstOrDefault() ?? throw new SiteNotFoundException($"Site not found");

      string? connectionString = _configuration.GetConnectionString("DefaultTenant")
        .Replace("Tenant_DB", $"Tenant_{site.Name}_DB") ?? null;

      var options = new DbContextOptionsBuilder<SiteDbContext>()
        .UseSqlServer(connectionString)
        .Options;
      var dbContext = new SiteDbContext(options);

      var users = dbContext.Users.ToList();

      dbContext.Dispose();

      return users.Select(x => new UserDTO
      {
        Id = x.Id,
        Email = x.Email,
        Name = x.Name,
        Role = x.Role != null
          ? new RoleDTO
          {
            Id = x.Role.Id,
            Name = x.Role.Name,
            Permissions = x.Role.Permissions.Select(y => new PermissionDTO { Id = y.Id, Name = y.Name }).ToList() ??
                          new List<PermissionDTO>()
          } : null,
      }).ToList();
    }

    public bool CreateNewSite(string ownerEmail, SiteDTO site, out string? errorMessage, out string? connectionString)
    {
      errorMessage = null;
      connectionString = null;
      var existingSites = _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>>
      {
        x => x.Name == site.Name ||
             x.Domain == site.Domain
      }).ToList();

      if (existingSites.Any())
      {
        errorMessage = existingSites.Where(x => x.Name == site.Name).Any()
          ? $"A site with the name {site.Name} already exists"
          : $"A site with the domain {site.Domain} already exists";
        return false;
      }

      connectionString = _configuration.GetConnectionString("DefaultTenant")
        .Replace("Tenant_DB", $"Tenant_{site.Name}_DB");

      var options = new DbContextOptionsBuilder<SiteDbContext>()
        .UseSqlServer(connectionString)
        .Options;
      var dbContext = new SiteDbContext(options);

      dbContext.Database.Migrate();

      dbContext.Dispose();

      var newSite = new TuPencaUy.Platform.DAO.Models.Site
      {
        ConnectionString = connectionString,
        Name = site.Name,
        Domain = site.Domain,
        Color = site.Color,
        AccessType = site.AccessType,
      };

      //TODO: Insert enums data

      _siteDAL.Insert(newSite);
      _siteDAL.SaveChanges();

      var owner = _userDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.User, bool>>>
          { x => x.Email == ownerEmail })
        .FirstOrDefault();

      if (owner.Sites is null || !owner.Sites.Any())
        owner.Sites = new List<TuPencaUy.Platform.DAO.Models.Site>() { newSite };
      else owner.Sites.Add(newSite);

      _userDAL.SaveChanges();

      return true;
    }
  }
}
