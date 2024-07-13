using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IPayoutService
  {
    List<PayoutDTO> GetPayouts(out int count, string? userEmail, int? eventId, string? transactionId, int? siteId, int? page, int? pageSize);

    PayoutDTO CreatePayout(string userEmail, int eventId, int siteId, decimal amount, string transactionID);
  }
}
