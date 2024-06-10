using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformEventService : IEventService
  {
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IGenericRepository<Sport> _sportDAL;
    private readonly IGenericRepository<Team> _teamDAL;
    private readonly IGenericRepository<Match> _matchDAL;
    public PlatformEventService(
      IGenericRepository<Event> eventDAL,
      IGenericRepository<Sport> sportDAL,
      IGenericRepository<Team> teamDAL,
      IGenericRepository<Match> matchDAL)
    {
      _eventDAL = eventDAL;
      _sportDAL = sportDAL;
      _teamDAL = teamDAL;
      _matchDAL = matchDAL;
    }
    public MatchDTO ModifyMatch(
      int idMatch,
      int? idFirstTeam,
      int? idSecondTeam,
      DateTime? date,
      int? firstTeamScore,
      int? secondTeamScore,
      int? sportId)
    {
      var originMatch = _matchDAL.Get(new List<Expression<Func<Match, bool>>> { x => x.Id == idMatch })
        .FirstOrDefault() ?? throw new MatchNotFoundException($"Match with id {idMatch} not found");

      bool modified = false;

      if (idFirstTeam != null && idFirstTeam != originMatch.FirstTeam_id)
      {
        originMatch.FirstTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.Id == idFirstTeam })
          .FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idFirstTeam} not found");
        modified = modified || true;
      }

      if (idSecondTeam != null && idSecondTeam != originMatch.SecondTeam_id)
      {
        originMatch.SecondTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.Id == idSecondTeam })
          .FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idSecondTeam} not found");
        modified = modified || true;
      }

      if (date != null && date != originMatch.Date)
      {
        originMatch.Date = date;
        modified = modified || true;
      }

      if (firstTeamScore != null && firstTeamScore != originMatch.FirstTeamScore)
      {
        originMatch.FirstTeamScore = firstTeamScore;
        modified = modified || true;
      }

      if (secondTeamScore != null && secondTeamScore != originMatch.SecondTeamScore)
      {
        originMatch.SecondTeamScore = secondTeamScore;
        modified = modified || true;
      }

      if (sportId != null && sportId != originMatch.Sport_id)
      {
        originMatch.Sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == sportId })
          .FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {sportId} not found");
        modified = modified || true;
      }

      if (modified)
      {
        _matchDAL.Update(originMatch);
        _matchDAL.SaveChanges();
      }

      return new MatchDTO
      {
        Id = idMatch,
        FirstTeam = originMatch.FirstTeam != null ? new TeamDTO
        {
          Id = originMatch.FirstTeam.Id,
          Name = originMatch.FirstTeam.Name,
          TeamType = originMatch.FirstTeam.TeamType,
          Sport = originMatch.FirstTeam.Sport != null ? new SportDTO
          {
            Id = originMatch.FirstTeam.Sport.Id,
            Name = originMatch.FirstTeam.Sport.Name,
            ExactPoints = originMatch.FirstTeam.Sport.ExactPoints,
            Tie = originMatch.FirstTeam.Sport.Tie,
            PartialPoints = originMatch.FirstTeam.Sport.PartialPoints,
          } : null,
          Logo = originMatch.FirstTeam.Logo,
        } : null,
        SecondTeam = originMatch.SecondTeam != null ? new TeamDTO
        {
          Id = originMatch.SecondTeam.Id,
          Name = originMatch.SecondTeam.Name,
          TeamType = originMatch.SecondTeam.TeamType,
          Sport = originMatch.SecondTeam.Sport != null ? new SportDTO
          {
            Id = originMatch.SecondTeam.Sport.Id,
            Name = originMatch.SecondTeam.Sport.Name,
            ExactPoints = originMatch.SecondTeam.Sport.ExactPoints,
            Tie = originMatch.SecondTeam.Sport.Tie,
            PartialPoints = originMatch.SecondTeam.Sport.PartialPoints,
          } : null,
          Logo = originMatch.SecondTeam.Logo,
        } : null,
        FirstTeamScore = originMatch.FirstTeamScore,
        SecondTeamScore = originMatch.SecondTeamScore,
        Date = originMatch.Date,
        Sport = originMatch.Sport != null ? new SportDTO
        {
          Id = originMatch.Sport.Id,
          Name = originMatch.Sport.Name,
          ExactPoints = originMatch.Sport.ExactPoints,
          Tie = originMatch.Sport.Tie,
          PartialPoints = originMatch.Sport.PartialPoints,
        } : null,
      };
    }
    public TeamDTO ModifyTeam(
      int idTeam,
      string? name,
      byte[]? logo,
      TeamTypeEnum? teamType,
      int? sportId)
    {
      var originTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.Id == idTeam})
        .FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idTeam} not found");

      bool modified = false;

      if(name != null && name != originTeam.Name)
      {
        originTeam.Name = name;
        modified = modified || true;
      }

      if (logo != null && logo != originTeam.Logo)
      {
        originTeam.Logo = logo;
        modified = modified || true;
      }

      if (teamType != null && teamType != originTeam.TeamType)
      {
        originTeam.TeamType = teamType;
        modified = modified || true;
      }

      if (sportId != null && sportId != originTeam.Sport_id)
      {
        originTeam.Sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == sportId})
          .FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {sportId} not found");
        modified = modified || true;
      }

      if (modified)
      {
        _teamDAL.Update(originTeam);
        _teamDAL.SaveChanges();
      }

      return new TeamDTO
      {
        Id = idTeam,
        Name = originTeam.Name,
        TeamType = originTeam.TeamType,
        Sport = originTeam.Sport != null ? new SportDTO
        {
          Id = originTeam.Sport.Id,
          Name = originTeam.Sport.Name,
          ExactPoints = originTeam.Sport.ExactPoints,
          Tie = originTeam.Sport.Tie,
          PartialPoints = originTeam.Sport.PartialPoints,
        } : null,
        Logo = originTeam.Logo,
      };
    }
    public SportDTO ModifySport(
      int idSport,
      string? name,
      bool? tie,
      int? exactPoints,
      int? partialPoints)
    {
      var originSport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == idSport })
        .FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {idSport} not found");

      bool modified = false;

      if(name != null && name != originSport.Name)
      {
        originSport.Name = name;
        modified = modified || true;
      }

      if (tie != null && tie != originSport.Tie)
      {
        originSport.Tie = tie.Value;
        modified = modified || true;
      }

      if (exactPoints != null && exactPoints != originSport.ExactPoints)
      {
        originSport.ExactPoints = exactPoints;
        modified = modified || true;
      }

      if (partialPoints != null && partialPoints != originSport.PartialPoints)
      {
        originSport.PartialPoints = partialPoints;
        modified = modified || true;
      }

      if (modified)
      {
        _sportDAL.Update(originSport);
        _sportDAL.SaveChanges();
      }

      return new SportDTO
      {
        Id = originSport.Id,
        Name = originSport.Name,
        ExactPoints = originSport.ExactPoints,
        Tie = originSport.Tie,
        PartialPoints = originSport.PartialPoints,
      };
    }
    public EventDTO ModifyEvent(
      int idEvent,
      string? name,
      DateTime? startDate,
      DateTime? endTime,
      float? comission,
      TeamTypeEnum? teamType)
    {
      var originEvent = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == idEvent })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event with id {idEvent} not found");

      bool modified = false;

      if(name != null && name != originEvent.Name)
      {
        originEvent.Name = name;
        modified = modified || true;
      }

      if (startDate != null && startDate != originEvent.StartDate)
      {
        originEvent.StartDate = startDate;
        modified = modified || true;
      }

      if (endTime != null && endTime != originEvent.EndDate)
      {
        originEvent.EndDate = endTime;
        modified = modified || true;
      }

      if (comission != null && comission != originEvent.Comission)
      {
        originEvent.Comission = comission;
        modified = modified || true;
      }

      if (teamType != null && teamType != originEvent.TeamType)
      {
        originEvent.TeamType = teamType;
        modified = modified || true;
      }

      if (modified)
      {
        _eventDAL.Update(originEvent);
        _eventDAL.SaveChanges();
      }

      return new EventDTO
      {
        Id = originEvent.Id,
        Name = originEvent.Name,
        EndDate = originEvent.EndDate,
        Comission = originEvent.Comission,
        StartDate = originEvent.StartDate,
        TeamType = originEvent.TeamType,
      };
    }
    public void DeleteMatch(int idMatch)
    {
      var match = _matchDAL.Get(new List<Expression<Func<Match, bool>>> { match => match.Id == idMatch })
        .FirstOrDefault() ?? throw new MatchNotFoundException($"Match with id {idMatch} not found");

      _matchDAL.Delete(match);
      _matchDAL.SaveChanges();
    }
    public void DeleteTeam(int idTeam)
    {
      var team = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { team => team.Id == idTeam })
        .FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idTeam} not found");

      _teamDAL.Delete(team);
      _teamDAL.SaveChanges();
    }
    public void DeleteSport(int idSport)
    {
      var sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { sport => sport.Id == idSport })
        .FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {idSport} not found");

      _sportDAL.Delete(sport);
      _sportDAL.SaveChanges();
    }
    public void DeleteEvent(int idEvent)
    {
      var ev = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { ev => ev.Id == idEvent })
        .FirstOrDefault() ?? throw new SportNotFoundException($"Event with id {idEvent} not found");

      var matches = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { ev => ev.Id == idEvent }).Select(x => x.Matches).FirstOrDefault();

      if (matches != null && matches.Any())
      {
        foreach (var match in matches)
        {
          _matchDAL.Delete(match.Id);
        }
      }

      _eventDAL.Delete(ev);
      _eventDAL.SaveChanges();
    }
    public MatchDTO GetMatch(int idMatch)
    {
      return _matchDAL
        .Get(new List<Expression<Func<Match, bool>>> { match => match.Id == idMatch })?
        .Select(match => new MatchDTO
        {
          Id = match.Id,
          FirstTeam = match.FirstTeam != null ? new TeamDTO
          {
            Id = match.FirstTeam.Id,
            Name = match.FirstTeam.Name,
            TeamType = match.FirstTeam.TeamType,
            Sport = match.FirstTeam.Sport != null ? new SportDTO
            {
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
            Id = match.SecondTeam.Id,
            Name = match.SecondTeam.Name,
            TeamType = match.SecondTeam.TeamType,
            Sport = match.SecondTeam.Sport != null ? new SportDTO
            {
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
            Id = match.Sport.Id,
            Name = match.Sport.Name,
            ExactPoints = match.Sport.ExactPoints,
            Tie = match.Sport.Tie,
            PartialPoints = match.Sport.PartialPoints,
          } : null,
        }).FirstOrDefault() ?? throw new MatchNotFoundException($"Match with id {idMatch} not found");
    }
    public TeamDTO GetTeam(int idTeam)
    {
      return _teamDAL
        .Get(new List<Expression<Func<Team, bool>>> { team => team.Id == idTeam })?
        .Select(team => new TeamDTO
        {
          Id = team.Id,
          Name = team.Name,
          TeamType = team.TeamType,
          Sport = team.Sport != null ? new SportDTO
          {
            Id = team.Sport.Id,
            Name = team.Sport.Name,
            ExactPoints = team.Sport.ExactPoints,
            Tie = team.Sport.Tie,
            PartialPoints = team.Sport.PartialPoints,
          } : null,
          Logo = team.Logo,
        }).FirstOrDefault() ?? throw new TeamNotFoundException($"Team with id {idTeam} not found");
    }
    public SportDTO GetSport(int idSport)
    {
      return _sportDAL
        .Get(new List<Expression<Func<Sport, bool>>> { sport => sport.Id == idSport })?
        .Select(s => new SportDTO
        {
          Id = s.Id,
          Name = s.Name,
          ExactPoints = s.ExactPoints,
          Tie = s.Tie,
          PartialPoints = s.PartialPoints,
        }).FirstOrDefault() ?? throw new SportNotFoundException($"Sport with id {idSport} not found");
    }
    public EventDTO GetEvent(int idEvent)
    {
      return _eventDAL
        .Get(new List<Expression<Func<Event, bool>>> { ev => ev.Id == idEvent })?
        .Select(ev => new EventDTO
        {
          Id = ev.Id,
          Name = ev.Name,
          EndDate = ev.EndDate,
          Comission = ev.Comission,
          StartDate = ev.StartDate,
          TeamType = ev.TeamType,
        }).FirstOrDefault() ?? throw new EventNotFoundException($"Event with id {idEvent} not found");
    }

    public EventDTO CreateEvent(string name, DateTime? startDate, DateTime? endDate, float? comission, TeamTypeEnum? teamType)
    {
      var existingEvent = _eventDAL.Get(new List<Expression<Func<Event, bool>>>
      {
        x => x.Name == name
      }).Any() ;

      if (existingEvent) throw new NameAlreadyInUseException($"An event with the name {name} already exists");

      var newEvent = new Event
      {
        Name = name,
        StartDate = startDate,
        EndDate = endDate,
        Comission = comission,
        TeamType = teamType,
        Instantiable = true
      };

      _eventDAL.Insert(newEvent);
      _eventDAL.SaveChanges();

      return new EventDTO
      {
        Id = newEvent.Id,
        Name = name,
        StartDate = startDate,
        EndDate = endDate,
        Comission = comission,
        TeamType = teamType,
        Instantiable = true
      };
    }

    public List<EventDTO> GetEvents(int page, int pageSize, out int count)
    {
      page = page > 0 ? page : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      count = _eventDAL.Get().Count();

      return _eventDAL.Get()
        .Select(x => new EventDTO
        {
          Id = x.Id,
          Name = x.Name,
          StartDate = x.StartDate,
          EndDate = x.EndDate,
          Comission = x.Comission,
          TeamType = x.TeamType
        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool CreateSport(SportDTO sportDTO, out string? errorMessage) {
      errorMessage = null;
      var existingSport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>>
      {
        x => x.Name == sportDTO.Name
      }).ToList();

      if (existingSport.Count != 0)
      {
        errorMessage = $"A sport with the name {sportDTO.Name} already exists";
        return false;
      }

      var newSport = new Sport
      {
        Name = sportDTO.Name,
        Tie = sportDTO.Tie,
        ExactPoints = sportDTO.ExactPoints,
        PartialPoints = sportDTO.PartialPoints,
      };

      _sportDAL.Insert(newSport);
      _sportDAL.SaveChanges();

      return true;
    }

    public List<SportDTO> GetSports(int page, int pageSize, out int count)
    {
      page = page > 0 ? page : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      count = _sportDAL.Get().Count();

      return _sportDAL.Get()
        .Select(x => new SportDTO
        {
          Id = x.Id,
          Name = x.Name,
          Tie = x.Tie,
          ExactPoints = x.ExactPoints,
          PartialPoints = x.PartialPoints,
        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool CreateTeam(TeamDTO teamDTO, out string? errorMessage)
    {
      errorMessage = null;
      var existingTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>>
      {
        x => x.Name == teamDTO.Name && x.Sport_id == teamDTO.Sport.Id
      }).ToList();

      if (existingTeam.Count != 0)
      {
        errorMessage = $"A team with the name {teamDTO.Name} already exists";
        return false;
      }

      var sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == teamDTO.Sport.Id }).FirstOrDefault();
      
      var newTeam = new Team
      {
        Name = teamDTO.Name,
        Logo = teamDTO.Logo,
        TeamType = teamDTO.TeamType,
        Sport_id = teamDTO.Sport.Id,
        Sport = sport
      };

      _teamDAL.Insert(newTeam);
      _teamDAL.SaveChanges();

      return true;
    }

    public List<TeamDTO> GetTeams(int page, int pageSize, out int count)
    {
      page = page > 0 ? page : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      count = _teamDAL.Get().Count();

      return _teamDAL.Get()
        .Select(x => new TeamDTO
        {
          Id = x.Id,
          Name = x.Name,
          Logo = x.Logo,
          TeamType = x.TeamType,
          Sport = x.Sport != null ? new SportDTO
          {
            Id = x.Sport.Id,
            Name = x.Sport.Name,
            ExactPoints = x.Sport.ExactPoints,
            Tie = x.Sport.Tie,
            PartialPoints = x.Sport.PartialPoints,
          } : null,
        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool CreateMatch(int eventID, MatchDTO matchDTO, out string? errorMessage)
    {
      errorMessage = null;

      Event? eventSearch = _eventDAL.Get(new List<Expression<Func<Event, bool>>>
      {
        x => x.Id == eventID
      }).ToList().FirstOrDefault();

      if (eventSearch == null)
      {
        errorMessage = $"Event {eventID} nof founded";
        return false;
      }

      var teams = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.Id == matchDTO.FirstTeam.Id || x.Id == matchDTO.SecondTeam.Id })
        .ToList();

      var firstTeam = teams.First(x => x.Id == matchDTO.FirstTeam.Id);
      var secondTeam = teams.First(x => x.Id == matchDTO.SecondTeam.Id);

      var sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == matchDTO.Sport.Id }).FirstOrDefault();

      var newMatch = new Match
      {
       Sport = sport,
       FirstTeam = firstTeam,
       SecondTeam = secondTeam,
       FirstTeamScore = matchDTO.FirstTeamScore,
       SecondTeamScore = matchDTO.SecondTeamScore,
       Date = matchDTO.Date,
      };

      _matchDAL.Insert(newMatch);
      _matchDAL.SaveChanges();
      if(eventSearch.Matches == null)
      {
        eventSearch.Matches = new List<Match>();
      }

      eventSearch.Matches.Add(newMatch);
      _eventDAL.Update(eventSearch);
      _eventDAL.SaveChanges();

      return true;
    }

    public List<MatchDTO> GetMatches(int page, int pageSize, out int count)
    {
      page = page > 0 ? page : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      count = _matchDAL.Get().Count();

      return _matchDAL.Get()
        .Select(match => new MatchDTO
        {
          Id = match.Id,
          FirstTeam = match.FirstTeam != null ? new TeamDTO
          {
            Id = match.FirstTeam.Id,
            Name = match.FirstTeam.Name,
            TeamType = match.FirstTeam.TeamType,
            Sport = match.FirstTeam.Sport != null ? new SportDTO
            {
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
            Id = match.SecondTeam.Id,
            Name = match.SecondTeam.Name,
            TeamType = match.SecondTeam.TeamType,
            Sport = match.SecondTeam.Sport != null ? new SportDTO
            {
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
            Id = match.Sport.Id,
            Name = match.Sport.Name,
            ExactPoints = match.Sport.ExactPoints,
            Tie = match.Sport.Tie,
            PartialPoints = match.Sport.PartialPoints,
          } : null,
        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
  }
}
