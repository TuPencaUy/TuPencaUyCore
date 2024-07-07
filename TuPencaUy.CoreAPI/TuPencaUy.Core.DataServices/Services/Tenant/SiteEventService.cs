using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SiteEventService : IEventService
  {
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IGenericRepository<Sport> _sportDAL;
    private readonly IGenericRepository<Team> _teamDAL;
    private readonly IGenericRepository<Match> _matchDAL;
    private readonly IGenericRepository<Bet> _betDAL;

    private int _page = 1;
    private int _pageSize = 10;
    public SiteEventService(
      IGenericRepository<Event> eventDAL,
      IGenericRepository<Sport> sportDAL,
      IGenericRepository<Team> teamDAL,
      IGenericRepository<Match> matchDAL,
      IGenericRepository<Bet> betDAL)
    {
      _eventDAL = eventDAL;
      _sportDAL = sportDAL;
      _teamDAL = teamDAL;
      _matchDAL = matchDAL;
      _betDAL = betDAL;
    }
    public Tuple<EventDTO, List<MatchDTO>> InstantiateEvent(EventDTO eventDTO, List<MatchDTO> matches)
    {
      Sport sport = _sportDAL
        .Get(new List<Expression<Func<Sport, bool>>> { sport => sport.RefSport == eventDTO.Sport.Id })?.FirstOrDefault();

      if (sport == null)
      {
        sport = new Sport
        {
          Name = eventDTO.Sport.Name,
          ExactPoints = eventDTO.Sport.ExactPoints,
          PartialPoints = eventDTO.Sport.PartialPoints,
          Tie = eventDTO.Sport.Tie,
          RefSport = eventDTO.Sport.Id.Value,
        };
        _sportDAL.Insert(sport);
        _sportDAL.SaveChanges();
      }


      var matchesToInsert = new List<Match>();
      foreach (var match in matches)
      {
        var firstTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.RefTeam == match.FirstTeam.Id }).FirstOrDefault();
        if (firstTeam == null)
        {
          firstTeam = new Team
          {
            Name = match.FirstTeam.Name,
            RefTeam = match.FirstTeam.Id.Value,
            Logo = match.FirstTeam.Logo,
            Sport = sport,
            TeamType = match.FirstTeam.TeamType,
          };
          _teamDAL.Insert(firstTeam);
          _teamDAL.SaveChanges();
        }

        var secondTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.RefTeam == match.SecondTeam.Id }).FirstOrDefault();
        if (secondTeam == null)
        {
          secondTeam = new Team
          {
            Name = match.SecondTeam.Name,
            RefTeam = match.SecondTeam.Id.Value,
            Logo = match.SecondTeam.Logo,
            Sport = sport,
            TeamType = match.SecondTeam.TeamType,
          };
          _teamDAL.Insert(secondTeam);
          _teamDAL.SaveChanges();
        }

        matchesToInsert.Add(new Match
        {
          Finished = match.Finished.Value,
          RefMatch = match.Id.Value,
          FirstTeam = firstTeam,
          SecondTeam = secondTeam,
          Sport = sport,
          SecondTeamScore = match.SecondTeamScore,
          FirstTeamScore = match.FirstTeamScore,
          Date = match.Date,
        });
      }

      Event ev = new Event
      {
        RefEvent = eventDTO.Id.Value,
        Comission = eventDTO.Comission,
        EndDate = eventDTO.EndDate,
        StartDate = eventDTO.StartDate,
        Sports = new List<Sport>() { sport },
        Instantiable = true,
        Name = eventDTO.Name,
        TeamType = eventDTO.TeamType,
        Matches = matchesToInsert
      };

      _eventDAL.Insert(ev);
      _eventDAL.SaveChanges();

      Tuple<EventDTO, List<MatchDTO>> result = new Tuple<EventDTO, List<MatchDTO>>(
        new EventDTO
        {
          Id = ev.Id,
          ReferenceEvent = ev.RefEvent,
          Name = ev.Name,
          Comission = ev.Comission,
          EndDate = ev.EndDate,
          StartDate = ev.StartDate,
          Instantiable = ev.Instantiable,
          MatchesCount = matchesToInsert.Count(),
          TeamType = ev.TeamType,
          Sport = new SportDTO
          {
            Id = ev.Id,
            ReferenceSport = ev.Sports.FirstOrDefault().Id,
            Name = ev.Sports.FirstOrDefault().Name,
            Tie = ev.Sports.FirstOrDefault().Tie,
            PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
            ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
          }
        },
        matchesToInsert.Select(x => new MatchDTO
        {
          Finished = x.Finished,
          Id = x.Id,
          ReferenceMatch = x.RefMatch,
          Date = x.Date,
          FirstTeam = new TeamDTO
          {
            Id = x.FirstTeam.Id,
            Logo = x.FirstTeam?.Logo,
            Name = x.FirstTeam?.Name,
            Sport = new SportDTO
            {
              Id = ev.Id,
              ReferenceSport = ev.Sports.FirstOrDefault().Id,
              Name = ev.Sports.FirstOrDefault().Name,
              Tie = ev.Sports.FirstOrDefault().Tie,
              PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
              ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
            },
            TeamType = x.FirstTeam?.TeamType,
            ReferenceTeam = x.FirstTeam.RefTeam
          },
          SecondTeam = new TeamDTO
          {
            Id = x.SecondTeam.Id,
            Logo = x.SecondTeam?.Logo,
            Name = x.SecondTeam?.Name,
            Sport = new SportDTO
            {
              Id = ev.Id,
              ReferenceSport = ev.Sports.FirstOrDefault().Id,
              Name = ev.Sports.FirstOrDefault().Name,
              Tie = ev.Sports.FirstOrDefault().Tie,
              PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
              ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
            },
            TeamType = x.SecondTeam?.TeamType,
            ReferenceTeam = x.SecondTeam.RefTeam
          },
          SecondTeamScore = x.SecondTeamScore,
          FirstTeamScore = x.FirstTeamScore,
          Sport = new SportDTO
          {
            Id = ev.Id,
            ReferenceSport = ev.Sports.FirstOrDefault().Id,
            Name = ev.Sports.FirstOrDefault().Name,
            Tie = ev.Sports.FirstOrDefault().Tie,
            PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
            ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
          }
        }).ToList()
        );

      return result;
    }
    public MatchDTO GetMatch(int idMatch)
    {
      return _matchDAL
        .Get(new List<Expression<Func<Match, bool>>> { match => match.Id == idMatch })?
        .Select(match => new MatchDTO
        {
          Finished = match.Finished,
          ReferenceMatch = match.RefMatch,
          Id = match.Id,
          FirstTeam = match.FirstTeam != null ? new TeamDTO
          {
            ReferenceTeam = match.FirstTeam.RefTeam,
            Id = match.FirstTeam.Id,
            Name = match.FirstTeam.Name,
            TeamType = match.FirstTeam.TeamType,
            Sport = match.FirstTeam.Sport != null ? new SportDTO
            {
              ReferenceSport = match.SecondTeam.Sport.RefSport,
              Id = match.FirstTeam.Sport.Id,
              Name = match.FirstTeam.Sport.Name,
              ExactPoints = match.FirstTeam.Sport.ExactPoints,
              Tie = match.FirstTeam.Sport.Tie,
              PartialPoints = match.FirstTeam.Sport.PartialPoints,
            } : null,
            Logo = match.FirstTeam.Logo,
          } : null,
          SecondTeam = match.SecondTeam != null ? new TeamDTO
          {
            ReferenceTeam = match.SecondTeam.RefTeam,
            Id = match.SecondTeam.Id,
            Name = match.SecondTeam.Name,
            TeamType = match.SecondTeam.TeamType,
            Sport = match.SecondTeam.Sport != null ? new SportDTO
            {
              ReferenceSport = match.SecondTeam.Sport.RefSport,
              Id = match.SecondTeam.Sport.Id,
              Name = match.SecondTeam.Sport.Name,
              ExactPoints = match.SecondTeam.Sport.ExactPoints,
              Tie = match.SecondTeam.Sport.Tie,
              PartialPoints = match.SecondTeam.Sport.PartialPoints,
            } : null,
            Logo = match.SecondTeam.Logo,
          } : null,
          FirstTeamScore = match.FirstTeamScore,
          SecondTeamScore = match.SecondTeamScore,
          Date = match.Date,
          Sport = match.Sport != null ? new SportDTO
          {
            ReferenceSport = match.Sport.RefSport,
            Id = match.Sport.Id,
            Name = match.Sport.Name,
            ExactPoints = match.Sport.ExactPoints,
            Tie = match.Sport.Tie,
            PartialPoints = match.Sport.PartialPoints,
          } : null,
        }).FirstOrDefault() ?? throw new MatchNotFoundException($"Match with id {idMatch} not found");
    }
    public List<MatchDTO> GetMatches(
      out int count,
      int? idTeam,
      int? otherIdTeam,
      int? eventId,
      int? sportId,
      DateTime? fromDate,
      DateTime? untilDate,
      bool? finished,
      int? page, int? pageSize)
    {
      SetPagination(page, pageSize);

      var conditions = new List<Expression<Func<Match, bool>>>();

      if (fromDate != null && untilDate != null && fromDate >= untilDate) throw new InvalidDateFilterException();

      if (fromDate != null) conditions.Add(x => x.Date >= fromDate);
      if (untilDate != null) conditions.Add(x => x.Date <= fromDate);

      if (idTeam != null && otherIdTeam != null && idTeam != otherIdTeam)
      {
        conditions.Add(
          x => (x.FirstTeam_id == idTeam && x.SecondTeam_id == otherIdTeam)
          || (x.FirstTeam_id == idTeam && x.SecondTeam_id == otherIdTeam));
      }
      else if (idTeam != null)
      {
        conditions.Add(x => x.FirstTeam_id == idTeam || x.SecondTeam_id == idTeam);
      }
      else if (otherIdTeam != null)
      {
        conditions.Add(x => x.FirstTeam_id == otherIdTeam || x.SecondTeam_id == otherIdTeam);
      }

      if (eventId != null) conditions.Add(x => x.Event_id == eventId);
      if (sportId != null) conditions.Add(x => x.Sport_id == sportId);

      IQueryable<MatchDTO> matches = _matchDAL.Get(conditions)
        .Select(match => new MatchDTO
        {
          Finished = match.Finished,
          ReferenceMatch = match.Id,
          Id = match.Id,
          FirstTeam = match.FirstTeam != null ? new TeamDTO
          {
            ReferenceTeam = match.FirstTeam.RefTeam,
            Id = match.FirstTeam.Id,
            Name = match.FirstTeam.Name,
            TeamType = match.FirstTeam.TeamType,
            Sport = match.FirstTeam.Sport != null ? new SportDTO
            {
              ReferenceSport = match.FirstTeam.Sport.RefSport,
              Id = match.FirstTeam.Sport.Id,
              Name = match.FirstTeam.Sport.Name,
              ExactPoints = match.FirstTeam.Sport.ExactPoints,
              Tie = match.FirstTeam.Sport.Tie,
              PartialPoints = match.FirstTeam.Sport.PartialPoints,
            } : null,
            Logo = match.FirstTeam.Logo,
          } : null,
          SecondTeam = match.SecondTeam != null ? new TeamDTO
          {
            ReferenceTeam = match.SecondTeam.RefTeam,
            Id = match.SecondTeam.Id,
            Name = match.SecondTeam.Name,
            TeamType = match.SecondTeam.TeamType,
            Sport = match.SecondTeam.Sport != null ? new SportDTO
            {
              ReferenceSport = match.SecondTeam.Sport.RefSport,
              Id = match.SecondTeam.Sport.Id,
              Name = match.SecondTeam.Sport.Name,
              ExactPoints = match.SecondTeam.Sport.ExactPoints,
              Tie = match.SecondTeam.Sport.Tie,
              PartialPoints = match.SecondTeam.Sport.PartialPoints,
            } : null,
            Logo = match.SecondTeam.Logo,
          } : null,
          FirstTeamScore = match.FirstTeamScore,
          SecondTeamScore = match.SecondTeamScore,
          Date = match.Date,
          Sport = match.Sport != null ? new SportDTO
          {
            ReferenceSport = match.Sport.RefSport,
            Id = match.Sport.Id,
            Name = match.Sport.Name,
            ExactPoints = match.Sport.ExactPoints,
            Tie = match.Sport.Tie,
            PartialPoints = match.Sport.PartialPoints,
          } : null,
        });

      count = matches.Count();

      return matches.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }
    public TeamDTO GetTeam(int idTeam)
    {
      return _teamDAL
        .Get(new List<Expression<Func<Team, bool>>> { team => team.Id == idTeam })?
        .Select(team => new TeamDTO
        {
          ReferenceTeam = team.RefTeam,
          Id = team.Id,
          Name = team.Name,
          TeamType = team.TeamType,
          Sport = team.Sport != null ? new SportDTO
          {
            ReferenceSport = team.Sport.RefSport,
            Id = team.Sport.Id,
            Name = team.Sport.Name,
            ExactPoints = team.Sport.ExactPoints,
            Tie = team.Sport.Tie,
            PartialPoints = team.Sport.PartialPoints,
          } : null,
          Logo = team.Logo,
        }).FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idTeam} not found");
    }
    public List<TeamDTO> GetTeams(
      out int count,
      string? name,
      int? sportId,
      TeamTypeEnum? teamType,
      int? page, int? pageSize)
    {
      SetPagination(page, pageSize);

      var conditions = new List<Expression<Func<Team, bool>>>();

      if (name != null) conditions.Add(x => x.Name == name);
      if (sportId != null) conditions.Add(x => x.Sport_id == sportId);
      if (teamType != null) conditions.Add(x => x.TeamType == teamType);

      IQueryable<TeamDTO> teams = _teamDAL.Get(conditions)
        .Select(x => new TeamDTO
        {
          ReferenceTeam = x.RefTeam,
          Id = x.Id,
          Name = x.Name,
          Logo = x.Logo,
          TeamType = x.TeamType,
          Sport = x.Sport != null ? new SportDTO
          {
            ReferenceSport = x.Sport.RefSport,
            Id = x.Sport.Id,
            Name = x.Sport.Name,
            ExactPoints = x.Sport.ExactPoints,
            Tie = x.Sport.Tie,
            PartialPoints = x.Sport.PartialPoints,
          } : null,
        });

      count = teams.Count();

      return teams.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }
    public SportDTO GetSport(int idSport)
    {
      return _sportDAL
        .Get(new List<Expression<Func<Sport, bool>>> { sport => sport.Id == idSport })?
        .Select(s => new SportDTO
        {
          ReferenceSport = s.RefSport,
          Id = s.Id,
          Name = s.Name,
          ExactPoints = s.ExactPoints,
          Tie = s.Tie,
          PartialPoints = s.PartialPoints,
        }).FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {idSport} not found");
    }
    public List<SportDTO> GetSports(out int count, string? name, int? page, int? pageSize)
    {
      SetPagination(page, pageSize);

      var conditions = new List<Expression<Func<Sport, bool>>>();

      if (name != null) conditions.Add(x => x.Name == name);

      IQueryable<SportDTO> sports = _sportDAL.Get(conditions)
        .Select(x => new SportDTO
        {
          ReferenceSport = x.RefSport,
          Id = x.Id,
          Name = x.Name,
          Tie = x.Tie,
          ExactPoints = x.ExactPoints,
          PartialPoints = x.PartialPoints,
        });

      count = sports.Count();

      return sports.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }
    public EventDTO GetEvent(int idEvent)
    {
      return _eventDAL
        .Get(new List<Expression<Func<Event, bool>>> { ev => ev.Id == idEvent })?
        .Select(ev => new EventDTO
        {
          ReferenceEvent = ev.RefEvent,
          Id = ev.Id,
          Name = ev.Name,
          EndDate = ev.EndDate,
          Comission = ev.Comission,
          StartDate = ev.StartDate,
          TeamType = ev.TeamType,
          Sport = ev.Sports.Select(x => new SportDTO
          {
            ReferenceSport = x.RefSport,
            Name = x.Name,
            Tie = x.Tie,
            Id = x.Id,
            ExactPoints = x.ExactPoints,
            PartialPoints = x.PartialPoints,
          }).FirstOrDefault(),
          MatchesCount = ev.Matches != null ? ev.Matches.Count() : 0,
        }).FirstOrDefault() ?? throw new EventNotFoundException($"Event with id {idEvent} not found");
    }
    public List<EventDTO> GetEvents(
      out int count,
      string? name,
      DateTime? fromDate,
      DateTime? untilDate,
      TeamTypeEnum? teamType,
      bool? instantiable,
      int? page, int? pageSize)
    {
      SetPagination(page, pageSize);

      if (fromDate != null && untilDate != null && fromDate >= untilDate) throw new InvalidDateFilterException();

      var conditions = new List<Expression<Func<Event, bool>>>();

      if (name != null) conditions.Add(x => x.Name == name);
      if (fromDate != null) conditions.Add(x => x.StartDate >= fromDate);
      if (untilDate != null) conditions.Add(x => x.EndDate <= fromDate);
      if (teamType != null) conditions.Add(x => x.TeamType == teamType);
      if (instantiable != null) conditions.Add(x => x.Instantiable == instantiable);


      IQueryable<EventDTO> events = _eventDAL.Get(conditions)
        .Select(x => new EventDTO
        {
          ReferenceEvent = x.RefEvent,
          Id = x.Id,
          Name = x.Name,
          StartDate = x.StartDate,
          EndDate = x.EndDate,
          Comission = x.Comission,
          TeamType = x.TeamType,
          Instantiable = x.Instantiable,
          Sport = x.Sports.Select(x => new SportDTO
          {
            ReferenceSport = x.RefSport,
            Name = x.Name,
            Tie = x.Tie,
            Id = x.Id,
            ExactPoints = x.ExactPoints,
            PartialPoints = x.PartialPoints,
          }).FirstOrDefault(),
          MatchesCount = x.Matches != null ? x.Matches.Count() : 0,
        });

      count = _eventDAL.Get(conditions).Count();

      return events.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }

    public MatchDTO CreateMatch(int eventID, int? firstTeamId, int? secondTeamId, int? firstTeamScore, int? secondTeamScore, int sportId, DateTime date, int? refMatch = null)
    {
      var ev = _eventDAL.Get([x => x.Id == eventID]).FirstOrDefault();

      if (ev == null) return new MatchDTO();

      Sport sport = _sportDAL
        .Get([sport => sport.RefSport == sportId])?.FirstOrDefault();

      var teamsId = new List<int> { firstTeamId.Value, secondTeamId.Value };

      var teams = _teamDAL
        .Get([team => teamsId.Contains(team.RefTeam)])?.ToList();

      var match = new Match
      {
        RefMatch = refMatch.Value,
        Event = ev,
        SecondTeam_id = secondTeamId,
        FirstTeamScore = firstTeamScore,
        SecondTeamScore = secondTeamScore,
        Date = date,
        Sport = sport,
        FirstTeam = teams.Where(x => x.RefTeam == firstTeamId).First(),
        SecondTeam = teams.Where(x => x.RefTeam == secondTeamId).First(),
        Finished = false,
      };

      _matchDAL.Insert(match);
      _matchDAL.SaveChanges();

      return null;
    }
    public void ModifyMatches(int? idFirstTeam, int? idSecondTeam, DateTime? date, int? firstTeamScore, int? secondTeamScore, int? sportId, bool? finished, int? refMatch = null)
    {
      var matches = _matchDAL.Get([x => x.RefMatch == refMatch]).ToList();

      foreach (var match in matches)
      {
        if (match.Sport.RefSport != sportId)
        {
          match.Sport = _sportDAL.Get([x => x.RefSport == sportId]).FirstOrDefault();
        }

        if (match.FirstTeam.RefTeam != idFirstTeam)
        {
          match.FirstTeam = _teamDAL.Get([x => x.RefTeam == idFirstTeam]).FirstOrDefault();
        }

        if (match.SecondTeam.RefTeam != idSecondTeam)
        {
          match.SecondTeam = _teamDAL.Get([x => x.RefTeam == idSecondTeam]).FirstOrDefault();
        }

        if (date.HasValue && date != match.Date) match.Date = date;
        if (firstTeamScore.HasValue && firstTeamScore != match.FirstTeamScore) match.FirstTeamScore = firstTeamScore;
        if (secondTeamScore.HasValue && secondTeamScore != match.SecondTeamScore) match.SecondTeamScore = secondTeamScore;
        if (finished.HasValue && match.Finished != finished)
        {
          var bets = _betDAL.Get([x => x.Match_id == match.Id]).ToList();

          foreach(var bet in bets)
          {
            if (bet.ScoreFirstTeam == match.FirstTeamScore && match.SecondTeamScore == bet.ScoreSecondTeam)
            {
              bet.Points = match.Sport.ExactPoints;
            }
            else if (
              (match.FirstTeamScore > match.SecondTeamScore && bet.ScoreFirstTeam > bet.ScoreSecondTeam) ||
              (match.FirstTeamScore < match.SecondTeamScore && bet.ScoreFirstTeam < bet.ScoreSecondTeam) ||
              (match.FirstTeamScore == match.SecondTeamScore && bet.ScoreFirstTeam == bet.ScoreSecondTeam))
            {
              bet.Points = match.Sport.PartialPoints;
            }

            _betDAL.Update(bet);
          }

          match.Finished = finished.Value;
        }


        _matchDAL.Update(match);
      }
      _matchDAL.SaveChanges();
    }


    #region Don't need implementation

    public MatchDTO ModifyMatch(int idMatch, int? idFirstTeam, int? idSecondTeam, DateTime? date, int? firstTeamScore, int? secondTeamScore, int? sportId, bool? finished, int? refMatch = null)
    {
      throw new NotImplementedException();
    }
    public EventDTO CreateEvent(string name, DateTime? startDate, DateTime? endDate, float? comission, TeamTypeEnum? teamType, int sportId)
    {
      throw new NotImplementedException();
    }

    public SportDTO CreateSport(string name, bool tie, int? exactPoints, int? partialPoints)
    {
      throw new NotImplementedException();
    }

    public TeamDTO CreateTeam(string name, byte[]? logo, int sportId, TeamTypeEnum? teamType)
    {
      throw new NotImplementedException();
    }

    public void DeleteEvent(int idEvent)
    {
      throw new NotImplementedException();
    }

    public void DeleteMatch(int idMatch, int? refMatch = null)
    {
      throw new NotImplementedException();
    }

    public void DeleteSport(int idSport)
    {
      throw new NotImplementedException();
    }

    public void DeleteTeam(int idTeam)
    {
      throw new NotImplementedException();
    }

    public EventDTO ModifyEvent(int idEvent, string? name, DateTime? startDate, DateTime? endTime, float? comission, TeamTypeEnum? teamType, bool? instantiable)
    {
      throw new NotImplementedException();
    }

    public SportDTO ModifySport(int idSport, string? name, bool? tie, int? exactPoints, int? partialPoints)
    {
      throw new NotImplementedException();
    }

    public TeamDTO ModifyTeam(int idTeam, string? name, byte[]? logo, TeamTypeEnum? teamType, int? sportId)
    {
      throw new NotImplementedException();
    }

    #endregion

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }

    public List<EventDTO> GetEvents(int refEventId)
    {
      IQueryable<EventDTO> events = _eventDAL.Get([x => x.RefEvent == refEventId]).Select(x => new EventDTO
      {
        ReferenceEvent = x.RefEvent,
        Id = x.Id,
        Name = x.Name,
        StartDate = x.StartDate,
        EndDate = x.EndDate,
        Comission = x.Comission,
        TeamType = x.TeamType,
        Instantiable = x.Instantiable,
        Sport = x.Sports.Select(x => new SportDTO
        {
          ReferenceSport = x.RefSport,
          Name = x.Name,
          Tie = x.Tie,
          Id = x.Id,
          ExactPoints = x.ExactPoints,
          PartialPoints = x.PartialPoints,
        }).FirstOrDefault(),
        MatchesCount = x.Matches != null ? x.Matches.Count() : 0,
      });

      return events.ToList();
    }

    public List<MatchDTO> GetMatches(int refMatchId)
    {
      IQueryable<MatchDTO> matches = _matchDAL.Get([x => x.RefMatch == refMatchId]).Select(match => new MatchDTO
      {
        Finished = match.Finished,
        ReferenceMatch = match.Id,
        Id = match.Id,
        FirstTeam = match.FirstTeam != null ? new TeamDTO
        {
          ReferenceTeam = match.FirstTeam.RefTeam,
          Id = match.FirstTeam.Id,
          Name = match.FirstTeam.Name,
          TeamType = match.FirstTeam.TeamType,
          Sport = match.FirstTeam.Sport != null ? new SportDTO
          {
            ReferenceSport = match.FirstTeam.Sport.RefSport,
            Id = match.FirstTeam.Sport.Id,
            Name = match.FirstTeam.Sport.Name,
            ExactPoints = match.FirstTeam.Sport.ExactPoints,
            Tie = match.FirstTeam.Sport.Tie,
            PartialPoints = match.FirstTeam.Sport.PartialPoints,
          } : null,
          Logo = match.FirstTeam.Logo,
        } : null,
        SecondTeam = match.SecondTeam != null ? new TeamDTO
        {
          ReferenceTeam = match.SecondTeam.RefTeam,
          Id = match.SecondTeam.Id,
          Name = match.SecondTeam.Name,
          TeamType = match.SecondTeam.TeamType,
          Sport = match.SecondTeam.Sport != null ? new SportDTO
          {
            ReferenceSport = match.SecondTeam.Sport.RefSport,
            Id = match.SecondTeam.Sport.Id,
            Name = match.SecondTeam.Sport.Name,
            ExactPoints = match.SecondTeam.Sport.ExactPoints,
            Tie = match.SecondTeam.Sport.Tie,
            PartialPoints = match.SecondTeam.Sport.PartialPoints,
          } : null,
          Logo = match.SecondTeam.Logo,
        } : null,
        FirstTeamScore = match.FirstTeamScore,
        SecondTeamScore = match.SecondTeamScore,
        Date = match.Date,
        Sport = match.Sport != null ? new SportDTO
        {
          ReferenceSport = match.Sport.RefSport,
          Id = match.Sport.Id,
          Name = match.Sport.Name,
          ExactPoints = match.Sport.ExactPoints,
          Tie = match.Sport.Tie,
          PartialPoints = match.Sport.PartialPoints,
        } : null,
      });

      return matches.ToList();
    }

    private void CalculatePoints(ref Bet bet, int firstTeamScore, int secondTeamScore, int partialPoints, int exactPoints)
    {
      int points = 0;

      if (bet.ScoreFirstTeam == firstTeamScore && secondTeamScore == bet.ScoreSecondTeam)
      {
        points = exactPoints;
      }
      else if (
        (firstTeamScore > secondTeamScore && bet.ScoreFirstTeam > bet.ScoreSecondTeam) ||
        (firstTeamScore < secondTeamScore && bet.ScoreFirstTeam < bet.ScoreSecondTeam) ||
        (firstTeamScore == secondTeamScore && bet.ScoreFirstTeam == bet.ScoreSecondTeam))
      {
        points = partialPoints;
      }

      bet.Points = points;
    }
  }
}
