using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreatePaymentRequest
  {
    [Required]
    public required int EventId { get; set; }
    [Required]
    public required string UserEmail { get; set; }
    [Required]
    public required decimal Amount { get; set; }
    [Required]
    public required string TransactionID { get; set; }
  }
}
