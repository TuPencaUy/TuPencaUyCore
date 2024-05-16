namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateEventRequest
  {
    public required string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float? Comission { get; set; }
  }
}
