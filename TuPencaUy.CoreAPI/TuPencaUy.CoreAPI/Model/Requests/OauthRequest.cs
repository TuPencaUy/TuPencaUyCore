using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class OauthRequest
  {
    [Required]
    public required string Token { get; set; }

    [Required]
    public required bool IsAllowedRegister { get; set; } = true;
  }
}
