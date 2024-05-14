namespace TuPencaUy.Core.API.Model.Responses
{
  public class ApiResponse
  {
    public bool Error { get; set; } = false;
    public dynamic? Data { get; set; }
    public string? Message { get; set; }
  }
}
