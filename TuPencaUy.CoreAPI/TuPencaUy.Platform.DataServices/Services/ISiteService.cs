using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface ISiteService
  {
    SiteDTO GetSiteByDomain(string domain);

    bool CreateNewSite(SiteDTO site, out string? errorMessage);
  }
}
