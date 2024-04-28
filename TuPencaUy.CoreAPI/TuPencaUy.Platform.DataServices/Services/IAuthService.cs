namespace TuPencaUy.Platform.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? Authenticate(LoginRequestDTO login);
    string GenerateToken(UserDTO user);
  }
}
