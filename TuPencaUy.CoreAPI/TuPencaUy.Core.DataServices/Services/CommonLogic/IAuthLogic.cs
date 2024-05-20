using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DataServices.Services.CommonLogic
{
  public interface IAuthLogic
  {
    string HashPassword(string password, string? hashedSalt = null);
    Tuple<string, DateTime> GenerateToken(UserDTO user, string? currentTenant = null);
  }
}
