namespace TuPencaUy.Core.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? SignUp(string email, string password, string name, bool? auth = false);
    UserDTO? Authenticate(string email, string password, bool? auth = false);
    UserDTO? Authenticate(string token, bool? auth = false);
  }
}
