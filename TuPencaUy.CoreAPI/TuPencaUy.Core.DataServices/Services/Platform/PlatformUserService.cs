using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.Enums;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformUserService : IUserService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    public PlatformUserService(IGenericRepository<User> userDAL, IGenericRepository<Role> roleDAL)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
    }

    public List<UserDTO> GetUsersByEvent(int eventId)
    {
      throw new NotImplementedException();
    }
    public UserDTO GetUserByEmail(string email)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select(x => new UserDTO
        {
          Name = x.Name,
          Email = email,
          Id = x.Id,
          Password = x.Password,
          Role = x.Role == null ? null : new RoleDTO
          {
            Name = x.Role.Name,
            Id = x.Role.Id,
            Permissions = x.Role.Permissions == null ? null :
              x.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
          },
        }) 
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }
    public RoleDTO GetRolesByUser(string email)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })?
        .Select(x => x.Role != null ? new RoleDTO
        {
          Id = x.Role.Id,
          Name = x.Role.Name,
          Permissions = x.Role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>()
        } : null)
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }

    public bool CreateUser(string email, string name, string? password, UserRoleEnum role)
    {
      Role userRole = _roleDAL.Get(new List<Expression<Func<Role, bool>>>
        {
          x => x.Id == (int)role
        }).FirstOrDefault();

      var newUser = new User
      {
        Email = email,
        Name = name,
        Password = password,
      };
      if (userRole is not null) newUser.Role = userRole;

      _userDAL.Insert(newUser);
      _userDAL.SaveChanges();

      return true;
    }
  }
}
