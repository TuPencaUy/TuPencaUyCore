using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;

namespace TuPencaUy.Core.DataServices.Services
{
  public class SiteService : ISiteService
  {
    private readonly IGenericRepository<Platform.DAO.Models.Site> _siteDAL;

    public SiteService(IGenericRepository<Platform.DAO.Models.Site> siteDAL)
    {
      _siteDAL = siteDAL;
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
  }
}
