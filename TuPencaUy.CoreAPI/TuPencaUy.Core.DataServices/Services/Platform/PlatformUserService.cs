using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices.Services.CommonLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformUserService : IUserService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IAuthLogic _authLogic;
    public PlatformUserService(
      IGenericRepository<User> userDAL,
      IGenericRepository<Role> roleDAL,
      IAuthLogic authLogic)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
      _authLogic = authLogic;
    }

    public UserDTO GetUserById(int id)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Id == id })
        .Select(x => new UserDTO
        {
          Name = x.Name,
          Email = x.Email,
          Id = id,
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

    public UserDTO ModifyUser(int userId, string? email, string? name, string? password, string? paypalEmail)
    {
      var dbUser = _userDAL.Get(new List<Expression<Func<User, bool>>> { user => user.Id == userId })
        .FirstOrDefault() ?? throw new UserNotFoundException();

      if (email is not null && email != dbUser.Email)
      {
        var userWithEmail = _userDAL.Get(new List<Expression<Func<User, bool>>>
        {
          user => user.Email == user.Email
        }).Any();
        if (userWithEmail)
        {
          throw new EmailAlreadyInUseException($"The email {email} is already in use");
        }
      }

      if (email is not null) dbUser.Email = email;
      if (name is not null) dbUser.Name = name;
      if (password is not null) dbUser.Password = _authLogic.HashPassword(password);
      if (paypalEmail is not null) dbUser.PaypalEmail = paypalEmail;

      if ((email is not null && email != dbUser.Email)
        || (name is not null && name != dbUser.Name)
        || (paypalEmail is not null && paypalEmail != dbUser.PaypalEmail)
        || (password is not null))
      {
        _userDAL.Update(dbUser);
        _userDAL.SaveChanges();
      }

      return new UserDTO
      {
        Name = dbUser.Name,
        Email = dbUser.Email,
        Id = dbUser.Id,
        Password = dbUser.Password,
        PaypalEmail = dbUser.PaypalEmail,
        Role = dbUser.Role == null ? null : new RoleDTO
        {
          Name = dbUser.Role.Name,
          Id = dbUser.Role.Id,
          Permissions = dbUser.Role.Permissions == null ? null :
              dbUser.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
        }
      };
    }

    public Tuple<UserDTO, EventDTO> SubscribeUser(int userId, int eventId)
    {
      throw new NotImplementedException();
    }
  }
}
