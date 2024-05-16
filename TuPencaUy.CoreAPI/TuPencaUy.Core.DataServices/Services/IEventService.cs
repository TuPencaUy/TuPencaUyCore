using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    bool CreateEvent(EventDTO eventDTO, out string? errorMessage);
    bool CreateSport(SportDTO sportDTO, out string? errorMessage);
    bool CreateTeam(TeamDTO teamDTO, out string? errorMessage);
    bool CreateMatch(int eventId, MatchDTO matchDTO, out string? errorMessage);
  }
}
