using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Platform.DataServices.Services
{
  public class UserService : IUserService
  {
    private readonly IGenericRepository<User> _userDAL;
    public UserService(IGenericRepository<User> userDAL)
    {
      _userDAL = userDAL;
    }
    public RoleDTO GetRolesByUser(string email)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })?
        .Select(x => x.Role != null ? new RoleDTO
        {
          Id = x.Role.Id,
          Name = x.Role.Name,
          Permissions = x.Role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name})
            .ToList() ?? new List<PermissionDTO> ()
        } : null)
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }
  }
}
