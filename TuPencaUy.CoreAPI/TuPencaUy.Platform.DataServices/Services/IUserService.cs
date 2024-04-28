using TuPencaUy.DTOs;

namespace TuPencaUy.Platform.DataServices.Services
{
  public interface IUserService
  {
    RoleDTO GetRolesByUser(string email);
  }
}
