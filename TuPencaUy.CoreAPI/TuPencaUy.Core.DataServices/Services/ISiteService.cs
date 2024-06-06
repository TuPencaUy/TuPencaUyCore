using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface ISiteService
  {
    SiteDTO GetSiteByDomain(string domain);

    bool CreateNewSite(string ownerEmail, SiteDTO site, out string? errorMessage, out string? connectionString);

    void DeleteSite(int siteID);

    void UpdateSite(SiteDTO site);
  }
}
