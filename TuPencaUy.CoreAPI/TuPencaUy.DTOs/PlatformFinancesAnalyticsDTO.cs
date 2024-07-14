namespace TuPencaUy.Core.DTOs
{
  public class PlatformFinancesAnalyticsDTO
  {
    public decimal TotalRaised {
      get
      {
        return Sites.Sum(s => s.TotalRaised);
      }
    }
    public List<SiteFinanceDTO> Sites {  get; set; }

    public List<EventFinanceDTO> Events {  get; set; }
  }
}
