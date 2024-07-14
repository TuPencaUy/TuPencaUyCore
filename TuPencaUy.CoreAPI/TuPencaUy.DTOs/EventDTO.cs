using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DTOs
{
  public class EventDTO
  {
    public int? Id { get; set; }
    public required string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Comission { get; set; }
    public TeamTypeEnum? TeamType { get; set; }
    public bool Instantiable { get; set; }
    public int MatchesCount { get; set; }
    public int? ReferenceEvent { get; set; }
    public SportDTO? Sport { get; set; }
    public int? Price { get; set; }
    public decimal? PrizePercentage { get; set; }
    public bool? Finished { get; set; }
  }
}
