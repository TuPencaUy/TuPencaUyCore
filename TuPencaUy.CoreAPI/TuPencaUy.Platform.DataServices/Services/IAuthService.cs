namespace TuPencaUy.Platform.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? Authenticate(LoginRequestDTO login);
    UserDTO? Authenticate(string token);
    string GenerateToken(UserDTO user);
  }
}
