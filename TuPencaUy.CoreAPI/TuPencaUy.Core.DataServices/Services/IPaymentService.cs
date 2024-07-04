using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IPaymentService
  {
    List<PaymentDTO> GetPayments(out int count, string? userEmail, int? eventId, string? transactionId, int? page, int? pageSize);

    PaymentDTO CreatePayment(string userEmail, int eventId, decimal amount, string transactionID);

    PaymentDTO ModifyPayment(string userEmail, int eventId, int? amount, string? transactionID);

    void DeletePayment(string userEmail, int eventId);
  }
}
