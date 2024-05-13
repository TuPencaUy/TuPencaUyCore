namespace TuPencaUy.Platform.DataServices.Services
{
  using System.Linq.Expressions;
  using System.Text;
  using Microsoft.Extensions.Configuration;
  using Microsoft.IdentityModel.Tokens;
  using System.IdentityModel.Tokens.Jwt;
  using System.Security.Claims;
  using TuPencaUy.DTOs;
  using TuPencaUy.Platform.DAO.Models;
  using System.Security.Cryptography;
  using Microsoft.AspNetCore.Cryptography.KeyDerivation;
  using TuPencaUy.Core.DataAccessLogic;
  using TuPencaUy.Core.Enums;
  using TuPencaUy.Exceptions;

  public class AuthService : IAuthService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IConfiguration _config;
    public AuthService(
      IGenericRepository<User> userDAL,
      IGenericRepository<Role> roleDAL,
      IConfiguration config)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
      _config = config;
    }
    public UserDTO? Authenticate(string email, string password)
    {
      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
          .FirstOrDefault();

      if (user == null) throw new InvalidCredentialsException();

      return VerifyPassword(password, user.Password) ? new UserDTO
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
    public Tuple<string, DateTime> GenerateToken(UserDTO user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.Id.ToString() ?? "undefined"),
        new Claim("currentTenant", ""), // TODO
      };
      var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:Audience"]));

      var token = new JwtSecurityToken(
        _config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: expires,
        signingCredentials: credentials);

      var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

      return new Tuple<string, DateTime>(stringToken, expires);
    }

    public UserDTO CreateNewUser(string email, string name)
    {
      Role role = _roleDAL
        .Get(new List<Expression<Func<Role, bool>>> { x => x.Id == (int)UserRoleEnum.BasicUser })
        .FirstOrDefault();

      _userDAL.Insert(new User
      {
        Email = email,
        Name = name,
        Role = role,
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
    private string HashPassword(string password)
    {
      byte[] salt = new byte[16];
      RandomNumberGenerator.Create().GetBytes(salt);

      string hashedPassword = Convert.ToBase64String(
          KeyDerivation.Pbkdf2(
              password: password,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 10000,
              numBytesRequested: 32
          )
      );

      string combinedHash = $"{Convert.ToBase64String(salt)}${hashedPassword}";

      return combinedHash;
    }
    private bool VerifyPassword(string hashedPassword, string password)
    {
      string[] hashParts = hashedPassword.Split('$');
      byte[] salt = Convert.FromBase64String(hashParts[0]);
      string storedHashedPassword = hashParts[1];

      string hashedPasswordToVerify = Convert.ToBase64String(
          KeyDerivation.Pbkdf2(
              password: password,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 10000,
              numBytesRequested: 32
          )
      );

      return storedHashedPassword.Equals(hashedPasswordToVerify);
    }
  }
}
