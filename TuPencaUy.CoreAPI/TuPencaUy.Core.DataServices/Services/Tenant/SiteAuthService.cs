namespace TuPencaUy.Core.DataServices.Services.Platform
{
  using System.Linq.Expressions;
  using System.Text;
  using Microsoft.Extensions.Configuration;
  using Microsoft.IdentityModel.Tokens;
  using System.IdentityModel.Tokens.Jwt;
  using System.Security.Claims;
  using TuPencaUy.DTOs;
  using TuPencaUy.Site.DAO.Models;
  using System.Security.Cryptography;
  using Microsoft.AspNetCore.Cryptography.KeyDerivation;
  using TuPencaUy.Core.DataAccessLogic;
  using TuPencaUy.Core.Enums;
  using TuPencaUy.Exceptions;
  using TuPencaUy.Core.DataServices.Services.CommonLogic;

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
    public UserDTO? Authenticate(string email, string password)
    {
      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
          .FirstOrDefault();

      if (user == null) throw new InvalidCredentialsException();

      return user.Password.Equals(_authLogic.HashPassword(password, user.Password.Split('$')[0])) ? new UserDTO
      {
        Email = email,
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
      } : null;

    }
    public UserDTO? Authenticate(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      try
      {
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

        var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .Select(x => new UserDTO
        {
          Email = userEmail,
          Id = x.Id,
          Name = x.Name,
          Role = x.Role != null ? new RoleDTO
          {
            Name = x.Role.Name,
            Id = x.Role.Id,
            Permissions = x.Role.Permissions
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

    public UserDTO? SignUp(string email, string password, string name)
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

      return new UserDTO
      {
        Email = email,
        Name = name,
        Role = role != null ? new RoleDTO
        {
          Name = role.Name,
          Id = role.Id,
          Permissions = role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>()
        } : null
      };
    }
  }
}
