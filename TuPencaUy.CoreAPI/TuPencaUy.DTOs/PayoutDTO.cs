using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class PayoutDTO
  {
    public int? Id { get; set; }
    public EventDTO Event { get; set; }
    public decimal? Amount { get; set; }
    public string? TransactionID { get; set; }
    public string? PaypalEmail { get; set; }
    public SiteDTO SiteDTO { get; set; }
  }
}
