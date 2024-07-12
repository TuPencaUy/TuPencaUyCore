using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IAnalyticsService
  {
    List<BetUserDTO> GetLeaderboard(out int count, int eventId, int? page = null, int? pageSize = null);
    List<BetMatchDTO> GetMatchBets(int? matchId);
    List<BetEventDTO> GetEventBets(int? eventId);
  }
}
