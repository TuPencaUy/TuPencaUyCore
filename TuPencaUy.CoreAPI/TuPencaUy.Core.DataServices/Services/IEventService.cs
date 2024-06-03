using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    bool CreateEvent(EventDTO eventDTO, out string? errorMessage);
    List<EventDTO> GetEvents(int page, int pageSize, out int count);
    bool CreateSport(SportDTO sportDTO, out string? errorMessage);
    List<SportDTO> GetSports(int page, int pageSize, out int count);
    bool CreateTeam(TeamDTO teamDTO, out string? errorMessage);
    List<TeamDTO> GetTeams(int page, int pageSize, out int count);
    bool CreateMatch(int eventId, MatchDTO matchDTO, out string? errorMessage);
    List<MatchDTO> GetMatches(int page, int pageSize, out int count);
    MatchDTO GetMatch(int idMatch);
    TeamDTO GetTeam(int idTeam);
    SportDTO GetSport(int idSport);
    EventDTO GetEvent(int idEvent);
  }
}
