namespace TuPencaUy.Core.DataServices.Services.Platform
{
  using System.Linq.Expressions;
  using System.IdentityModel.Tokens.Jwt;
  using TuPencaUy.DTOs;
  using TuPencaUy.Platform.DAO.Models;
  using TuPencaUy.Core.DataAccessLogic;
  using TuPencaUy.Core.Enums;
  using TuPencaUy.Exceptions;
  using TuPencaUy.Core.DataServices.Services.CommonLogic;
  using TuPencaUy.Core.DTOs;

  public class PlatformAuthService : IAuthService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IAuthLogic _authLogic;
    public PlatformAuthService(
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
      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select(user => new UserDTO
        {
          Site = user.Sites != null ? user.Sites.Where(site => !site.Inactive)
            .Select(site => new SiteDTO
            {
              AccessType = site.AccessType,
              Domain = site.Domain,
              ConnectionString = site.ConnectionString,
              Color = site.Color,
              Id = site.Id,
              Name = site.Name,
            })
            .FirstOrDefault() : null,
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

        var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .Select(user => new UserDTO
        {
          Site = user.Sites != null ? user.Sites.Where(site => !site.Inactive)
            .Select(site => new SiteDTO
            {
              AccessType = site.AccessType,
              Domain = site.Domain,
              ConnectionString = site.ConnectionString,
              Color = site.Color,
              Id = site.Id,
              Name = site.Name,
            })
            .FirstOrDefault() : null,
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

        if (user == null)
        {
          var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
          return CreateNewUser(userEmail, userName);
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

      return CreateNewUser(email, name, _authLogic.HashPassword(password));
    }

    private UserDTO CreateNewUser(string email, string name, string password = null)
    {
      Role role = _roleDAL
        .Get(new List<Expression<Func<Role, bool>>> { x => x.Id == (int)UserRoleEnum.BasicUser })
        .FirstOrDefault();

      _userDAL.Insert(new User
      {
        Email = email,
        Name = name,
        Role = role,
        Password = password
      });

      _userDAL.SaveChanges();

      var userDTO =  new UserDTO
      {
        Email = email,
        Name = name
      };

      if (role != null) {
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
