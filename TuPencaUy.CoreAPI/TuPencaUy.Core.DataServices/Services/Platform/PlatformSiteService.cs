using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
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

    public List<SiteDTO> GetSites()
    {
      return _siteDAL.Get().Select(x => new SiteDTO
      {
        Id = x.Id,
        Name = x.Name,
        Domain = x.Domain,
        ConnectionString = x.ConnectionString,
        Color = x.Color,
        AccessType = x.AccessType,
        UniqueID = x.UniqueID,
      }).ToList();
    }

    public SiteDTO GetSiteByDomain(string domain)
    {
      return _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>> { x => x.Domain == domain })
        .Select(x => new SiteDTO
        {
          Id = x.Id,
          Name = x.Name,
          Domain = domain,
          ConnectionString = x.ConnectionString,
          Color = x.Color,
          AccessType = x.AccessType,
          PaypalEmail = x.PaypalEmail,
          UniqueID = x.UniqueID,
        })
        .FirstOrDefault() ?? throw new SiteNotFoundException($"No site was found with domain {domain}");
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
        UniqueID = site.AccessType == SiteAccessTypeEnum.ByInvite ? new Guid().ToString() : null
      };

      _siteDAL.Insert(newSite);
      _siteDAL.SaveChanges();

      var owner = _userDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.User, bool>>> { x => x.Email == ownerEmail })
        .FirstOrDefault();

      if (owner.Sites is null || !owner.Sites.Any()) owner.Sites = new List<TuPencaUy.Platform.DAO.Models.Site>() { newSite };
      else owner.Sites.Add(newSite);

      _userDAL.SaveChanges();

      return true;
    }

    public void DeleteSite(int siteID)
    {
      var site = _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>>
      {
        x => x.Id == siteID
      }).FirstOrDefault() ?? throw new SiteNotFoundException();

      _siteDAL.Delete(site);
      _siteDAL.SaveChanges();
    }

    public void UpdateSite(SiteDTO siteDTO)
    {
      var site = _siteDAL.Get(new List<Expression<Func<TuPencaUy.Platform.DAO.Models.Site, bool>>>
      {
        x => x.Id == siteDTO.Id
      }).FirstOrDefault() ?? throw new SiteNotFoundException();

      site.Domain = siteDTO.Domain;
      site.Color = siteDTO.Color;
      site.AccessType = siteDTO.AccessType;

      if (siteDTO.AccessType == SiteAccessTypeEnum.ByInvite && string.IsNullOrEmpty(site.UniqueID)) site.UniqueID = new Guid().ToString();

      if(siteDTO.PaypalEmail != null) site.PaypalEmail = siteDTO.PaypalEmail;

      _siteDAL.Update(site);
      _siteDAL.SaveChanges();
    }
  }
}
