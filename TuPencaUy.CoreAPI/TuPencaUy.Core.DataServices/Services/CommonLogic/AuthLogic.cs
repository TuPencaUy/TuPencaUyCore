using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DataServices.Services.CommonLogic
{
  public class AuthLogic : IAuthLogic
  {
    private readonly IConfiguration _config;
    public AuthLogic(IConfiguration config) => _config = config;
    public string HashPassword(string password, string? hashedSalt = null)
    {
      byte[] salt = new byte[16];
      if (hashedSalt is not null) salt = Convert.FromBase64String(hashedSalt);
      else RandomNumberGenerator.Create().GetBytes(salt);

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

    public Tuple<string, DateTime> GenerateToken(UserDTO user, string? currentTenant = null)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role?.Id.ToString() ?? "undefined"),
        new Claim("currentTenant", currentTenant ?? string.Empty),
      };
      var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:MinutesTokenLifeTime"]));

      var token = new JwtSecurityToken(
        _config["JwtSettings:Issuer"],
        _config["JwtSettings:Audience"],
        claims,
        expires: expires,
        signingCredentials: credentials);

      var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

      return new Tuple<string, DateTime>(stringToken, expires);
    }
  }
}
