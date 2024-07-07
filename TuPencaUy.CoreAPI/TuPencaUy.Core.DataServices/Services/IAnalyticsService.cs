using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IAnalyticsService
  {
    List<BetUserDTO> GetLeaderboard(out int count, int eventId, int? page = null, int? pageSize = null);
  }
}
