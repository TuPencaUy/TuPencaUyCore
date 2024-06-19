namespace TuPencaUy.Core.DataServices.Services.Platform
{
  using System.Linq.Expressions;
  using System.IdentityModel.Tokens.Jwt;
  using TuPencaUy.DTOs;
  using TuPencaUy.Site.DAO.Models;
  using TuPencaUy.Core.DataAccessLogic;
  using TuPencaUy.Core.Enums;
  using TuPencaUy.Exceptions;
  using TuPencaUy.Core.DataServices.Services.CommonLogic;
  using TuPencaUy.Core.Exceptions;

  public class SiteAuthService : IAuthService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IAuthLogic _authLogic;
    public SiteAuthService(
      IGenericRepository<User> userDAL,
      IGenericRepository<Role> roleDAL,
      IAuthLogic authLogic)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
      _authLogic = authLogic;
    }
    public UserDTO? Authenticate(string email, string password, bool? auth = false)
    {
      if (auth != null && auth.Value)
      {
        var accessStatus = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email }).Select(x => x.AccessRequest.AccessStatus)
          .FirstOrDefault();
        if (accessStatus != AccessStatusEnum.Accepted) throw new UserNotAdmitedException($"User {email} not admited.");
      }

      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select( user => new UserDTO
        {
          Password = user.Password,
          Email = user.Email,
          Id = user.Id,
          Name = user.Name,
          Role = user.Role != null ? new RoleDTO
          {
            Name = user.Role.Name,
            Id = user.Role.Id,
            Permissions = user.Role.Permissions
             .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
             .ToList() ?? new List<PermissionDTO>()
          } : null,
        })
        .FirstOrDefault();

      if (user == null) throw new InvalidCredentialsException();

      return user.Password.Equals(_authLogic.HashPassword(password, user.Password.Split('$')[0])) ? user : null;
    }
    public UserDTO? Authenticate(string token, bool? auth = false)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      try
      {
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

        var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .Select(user => new UserDTO
        {
          Email = user.Email,
          Id = user.Id,
          Name = user.Name,
          Role = user.Role != null ? new RoleDTO
          {
            Name = user.Role.Name,
            Id = user.Role.Id,
            Permissions = user.Role.Permissions
             .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
             .ToList() ?? new List<PermissionDTO>()
          } : null,
        })
        .FirstOrDefault();
        
        if (user == null)
        {
          var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
          return CreateNewUser(userEmail, userName, null, auth);
        }

        if (auth != null && auth.Value)
        {
          var accessStatus = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail }).Select(x => x.AccessRequest.AccessStatus)
            .FirstOrDefault();
          if (accessStatus != AccessStatusEnum.Accepted) throw new UserNotAdmitedException($"User {userEmail} not admited.");
        }

        return user;
      }
      catch (Exception)
      {
        throw;
      }
    }
    public UserDTO? SignUp(string email, string password, string name, bool? auth = false)
    {
      var existingUser = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email });
      if (existingUser.Any()) return null;

      return CreateNewUser(email, name, _authLogic.HashPassword(password), auth);
    }
    private UserDTO CreateNewUser(string email, string name, string password = null, bool? auth = false)
    {
      Role role = _roleDAL
        .Get(new List<Expression<Func<Role, bool>>> { x => x.Id == (int)UserRoleEnum.BasicUser })
        .FirstOrDefault();

      var user = new User
      {
        Email = email,
        Name = name,
        Role = role,
        Password = password,
      };

      if (auth != null && auth.Value)
      {
        user.AccessRequest = new AccessRequest
        {
          User_email = email,
          AccessStatus = AccessStatusEnum.Pending,
          RequestTime = DateTime.Now,
        };
      }

      _userDAL.Insert(user);
      _userDAL.SaveChanges();

      var userDTO = new UserDTO
      {
        Email = email,
        Name = name
      };

      if (role != null)
      {
        userDTO.Role = new RoleDTO
        {
          Name = role.Name,
          Id = role.Id
        };

        if (role.Permissions != null)
        {
          userDTO.Role.Permissions = role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>();
        }
      }

      return userDTO;
    }
  }
}
