using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IBetService
  {
    List<BetDTO> GetBets(out int count, string? userEmail, int? matchId, int? eventId, int? page, int? pageSize);

    BetDTO CreateBet(string userEmail, int matchId, int eventId, int firstTeamScore, int secondTeamScore);

    BetDTO ModifyBet(string userEmail, int matchId, int eventId, int? firstTeamScore, int? secondTeamScore);

    void DeleteBet(string userEmail, int matchId, int eventId);

    void UpdatePoints(string userEmail, int matchId, int eventId);

    EventPaymentDTO EndEvent(int eventId);
  }
}
