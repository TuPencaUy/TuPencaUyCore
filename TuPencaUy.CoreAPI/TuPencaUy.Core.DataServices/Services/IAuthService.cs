using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? SignUp(string email, string password, string name, SiteAccessTypeEnum siteAccess);
    UserDTO? Authenticate(string email, string password, SiteAccessTypeEnum siteAccess);
    UserDTO? Authenticate(string token, SiteAccessTypeEnum siteAccess, bool isAllowedRegister);
  }
}
