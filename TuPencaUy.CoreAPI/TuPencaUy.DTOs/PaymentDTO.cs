using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class PaymentDTO
  {
    public int? Id { get; set; }
    public EventDTO Event { get; set; }
    public UserDTO User { get; set; }
    public decimal? Amount { get; set; }
    public string? TransactionID { get; set; }
    public string? User_email { get; set; }
  }
}
