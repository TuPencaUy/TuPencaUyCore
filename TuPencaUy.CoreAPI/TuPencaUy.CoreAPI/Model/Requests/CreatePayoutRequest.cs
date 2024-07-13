using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreatePayoutRequest
  {
    [Required]
    public required int SiteId { get; set; }
    [Required]
    public required string PaypalEmail { get; set; }
    [Required]
    public required decimal Amount { get; set; }
    [Required]
    public required string TransactionID { get; set; }
    [Required]
    public required int EventId { get; set; }
  }
}
