using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    EventDTO CreateEvent(string name, DateTime? startDate, DateTime? endDate, float? comission, TeamTypeEnum? teamType);
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
    void DeleteMatch(int idMatch);
    void DeleteTeam(int idTeam);
    void DeleteSport(int idSport);
    void DeleteEvent(int idEvent);
    MatchDTO ModifyMatch(
      int idMatch,
      int? idFirstTeam,
      int? idSecondTeam,
      DateTime? date,
      int? firstTeamScore,
      int? secondTeamScore,
      int? sportId);
    TeamDTO ModifyTeam(
      int idTeam,
      string? name,
      byte[]? logo,
      TeamTypeEnum? teamType,
      int? sportId);
    SportDTO ModifySport(
      int idSport,
      string? name,
      bool? tie,
      int? exactPoints,
      int? partialPoints);
    EventDTO ModifyEvent(
      int idEvent,
      string? name,
      DateTime? startDate,
      DateTime? endTime,
      float? comission,
      TeamTypeEnum? teamType);
  }
}
