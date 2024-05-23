using TuPencaUy.Core.DTOs;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface ISiteService
  {
    SiteDTO GetSiteByDomain(string domain);

    bool CreateNewSite(string ownerEmail, SiteDTO site, out string? errorMessage, out string? connectionString);

    List<UserDTO> GetUsers(string domain);
  }
}
