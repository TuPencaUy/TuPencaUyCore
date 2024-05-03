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
  using TuPencaUy.Platform.DAO.Models.Logic;
  public class AuthService : IAuthService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IConfiguration _config;
    public AuthService(IGenericRepository<User> userDAL, IConfiguration config)
    {
      _userDAL = userDAL;
      _config = config;
    }
    public UserDTO? Authenticate(LoginRequestDTO login)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == login.Email && x.Password == login.Password })
        .Select(x => new UserDTO
        {
          Email = login.Email,
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
    }
    public UserDTO? Authenticate(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_config["Auth0:SecretKey"]);

      try
      {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
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
      }
      catch(Exception)
      {
        throw;
      }
    }
    public string GenerateToken(UserDTO user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.Id.ToString() ?? "undefined"),
      };

      var token = new JwtSecurityToken(
        _config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: DateTime.UtcNow.AddMinutes(10),
        signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
