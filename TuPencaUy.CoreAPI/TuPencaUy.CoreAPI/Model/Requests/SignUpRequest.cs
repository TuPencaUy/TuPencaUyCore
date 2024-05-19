namespace TuPencaUy.Core.API.Model.Requests
{
  public class SignUpRequest
  {
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
  }
}
