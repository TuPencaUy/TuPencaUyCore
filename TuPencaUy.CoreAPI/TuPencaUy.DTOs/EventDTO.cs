using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DTOs
{
  public class EventDTO
  {
    public int? Id { get; set; }
    public required string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float? Comission { get; set; }
    public TeamTypeEnum? TeamType { get; set; }
    public bool Instantiable { get; set; }
  }
}
