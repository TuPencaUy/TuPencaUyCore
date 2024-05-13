using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Site.DAO.Models.Data;

namespace TuPencaUy.Core.DataServices.Services
{
  public class SiteService : ISiteService
  {
    private readonly IGenericRepository<Platform.DAO.Models.Site> _siteDAL;
    private readonly IConfiguration _configuration;
    public SiteService(IGenericRepository<Platform.DAO.Models.Site> siteDAL, IConfiguration configuration)
    {
      _siteDAL = siteDAL;
      _configuration = configuration;
    }

    public SiteDTO GetSiteByDomain(string domain)
    {
      return _siteDAL.Get(new List<Expression<Func<Platform.DAO.Models.Site, bool>>> { x => x.Domain == domain })
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

    public bool CreateNewSite(SiteDTO site, out string? errorMessage)
    {
      errorMessage = null;
      var existingSites = _siteDAL.Get(new List<Expression<Func<Platform.DAO.Models.Site, bool>>>
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

      var connString = _configuration.GetConnectionString("DefaultTenant")
        .Replace("Tenant_DB", $"Tenant_{site.Name}_DB");

      var options = new DbContextOptionsBuilder<SiteDbContext>()
            .UseSqlServer(connString)
            .Options;
      var dbContext = new SiteDbContext(options);

      dbContext.Database.EnsureCreated();
      dbContext.Database.Migrate();

      var newSite = new Platform.DAO.Models.Site
      {
        ConnectionString = connString,
        Name = site.Name,
        Domain = site.Domain,
        Color = site.Color,
        AccessType = site.AccessType,
      };

      //TODO: Insert enums data

      _siteDAL.Insert(newSite);
      _siteDAL.SaveChanges();

      return true;
    }
  }
}
