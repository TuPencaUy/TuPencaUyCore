using System.Text.Json.Serialization;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class UserRequest
  {
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PaypalEmail { get; set; }
  }
}
