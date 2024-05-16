namespace TuPencaUy.Core.DTOs
{
  public class TeamDTO
  {
    public int? ID { get; set; }
    public required string Name { get; set; }
    public byte[]? Logo { get; set; }
  }
}
