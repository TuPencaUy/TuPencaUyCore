namespace TuPencaUy.Core.DTOs
{
  public class EventDTO
  {
    public required string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float? Comission { get; set; }
  }
}
