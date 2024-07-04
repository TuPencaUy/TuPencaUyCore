using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class PaymentDTO
  {
    public EventDTO Event { get; set; }
    public UserDTO User { get; set; }
    public decimal? Amount { get; set; }
    public string? TransactionID { get; set; }
  }
}
