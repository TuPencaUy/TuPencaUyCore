using TuPencaUy.Core.Enums;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IUserService
  {
    RoleDTO GetRolesByUser(string email);
    List<UserDTO> GetUsersByEvent(int eventId);
    UserDTO GetUserById(int id);

    UserDTO GetUserByEmail(string email);
    bool CreateUser(string email, string name, string? password, UserRoleEnum role);
    UserDTO ModifyUser(int userId, string? email, string? name, string? password);
  }
}
