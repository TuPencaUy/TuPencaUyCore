using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IEventService
  {
    Tuple<EventDTO, List<MatchDTO>> InstantiateEvent(EventDTO eventDTO, List<MatchDTO> matches, int price, decimal prizePercentage);
    EventDTO CreateEvent(
      string name,
      DateTime? startDate,
      DateTime? endDate,
      decimal? comission,
      TeamTypeEnum? teamType,
      int sportId);
    List<EventDTO> GetEvents(
      out int count,
      string? name,
      DateTime? fromDate,
      DateTime? untilDate,
      TeamTypeEnum? teamType,
      bool? instantiable,
      int? page, int? pageSize);
    List<EventDTO> GetEvents(int refEventId);
    SportDTO CreateSport(string name, bool tie, int? exactPoints, int? partialPoints);
    List<SportDTO> GetSports(out int count, string? name, int? page, int? pageSize);
    TeamDTO CreateTeam(string name, byte[]? logo, int sportId, TeamTypeEnum? teamType);
    List<TeamDTO> GetTeams(
      out int count,
      string? name,
      int? sportId,
      TeamTypeEnum? teamType,
      int? page, int? pageSize);
    MatchDTO CreateMatch(
      int eventID,
      int? firstTeamId,
      int? secondTeamId,
      int? firstTeamScore,
      int? secondTeamScore,
      int sportId,
      DateTime date,
      int? refMatch = null);
    List<MatchDTO> GetMatches(
      out int count,
      int? idTeam,
      int? otherIdTeam,
      int? eventId,
      int? sportId,
      DateTime? fromDate,
      DateTime? untilDate,
      bool? finished,
      int? page, int? pageSize);

    List<MatchDTO> GetMatches(int refMatchId);
    MatchDTO GetMatch(int idMatch);
    TeamDTO GetTeam(int idTeam);
    SportDTO GetSport(int idSport);
    EventDTO GetEvent(int idEvent);
    void DeleteMatch(int idMatch, int? refMatch = null);
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
      int? sportId,
      bool? finished,
      int? refMatch = null);
    void ModifyMatches(
      int? idFirstTeam,
      int? idSecondTeam,
      DateTime? date,
      int? firstTeamScore,
      int? secondTeamScore,
      int? sportId,
      bool? finished,
      int? refMatch = null);
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
      decimal? comission,
      TeamTypeEnum? teamType,
      bool? instantiable);
  }
}
