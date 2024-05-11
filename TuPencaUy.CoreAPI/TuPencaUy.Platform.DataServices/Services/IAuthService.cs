namespace TuPencaUy.Platform.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? Authenticate(LoginRequestDTO login);
    UserDTO? Authenticate(string token);
    Tuple<string, DateTime> GenerateToken(UserDTO user, string? currentTenant = null);
  }
}
