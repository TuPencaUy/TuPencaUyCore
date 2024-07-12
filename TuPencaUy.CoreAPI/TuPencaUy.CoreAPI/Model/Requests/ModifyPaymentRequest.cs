using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class ModifyPaymentRequest
  {
    [Required]
    public required int EventId { get; set; }
    [Required]
    public required string UserEmail { get; set; }
    public int? amount { get; set; }
    public string? transactionId { get; set; }
  }
}
