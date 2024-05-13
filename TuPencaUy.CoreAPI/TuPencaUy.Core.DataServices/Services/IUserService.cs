using TuPencaUy.Core.Enums;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IUserService
  {
    RoleDTO GetRolesByUser(string email);
    bool CreateUser(string email, string name, UserRoleEnum role);
  }
}
