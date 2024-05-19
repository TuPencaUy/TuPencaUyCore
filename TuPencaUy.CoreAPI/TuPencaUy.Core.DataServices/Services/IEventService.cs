using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    bool CreateEvent(EventDTO eventDTO, out string? errorMessage);
    List<EventDTO> GetEvents();
    bool CreateSport(SportDTO sportDTO, out string? errorMessage);
    List<SportDTO> GetSports();
    bool CreateTeam(TeamDTO teamDTO, out string? errorMessage);
    List<TeamDTO> GetTeams();
    bool CreateMatch(int eventId, MatchDTO matchDTO, out string? errorMessage);
    List<MatchDTO> GetMatches();
  }
}
