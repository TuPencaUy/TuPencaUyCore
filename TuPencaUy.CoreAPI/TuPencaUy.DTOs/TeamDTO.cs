using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DTOs
{
  public class TeamDTO
  {
    public int? ID { get; set; }
    public required string Name { get; set; }
    public byte[]? Logo { get; set; }
    public TeamTypeEnum TeamType { get; set; }
  }
}
