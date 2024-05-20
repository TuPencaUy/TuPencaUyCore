using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    bool CreateEvent(EventDTO eventDTO, out string? errorMessage);
    List<EventDTO> GetEvents(int page, int pageSize);
    bool CreateSport(SportDTO sportDTO, out string? errorMessage);
    List<SportDTO> GetSports(int page, int pageSize);
    bool CreateTeam(TeamDTO teamDTO, out string? errorMessage);
    List<TeamDTO> GetTeams(int page, int pageSize);
    bool CreateMatch(int eventId, MatchDTO matchDTO, out string? errorMessage);
    List<MatchDTO> GetMatches(int page, int pageSize);
  }
}
