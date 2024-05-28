using System.Linq.Expressions;
using TuPencaUy.Core.DAO;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
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

    public bool CreateEvent(EventDTO eventDTO, out string? errorMessage)
    {
      errorMessage = null;
      var existingEvent = _eventDAL.Get(new List<Expression<Func<Event, bool>>>
      {
        x => x.Name == eventDTO.Name
      }).ToList();

      if (existingEvent.Count != 0)
      {
        errorMessage = $"An event with the name {eventDTO.Name} already exists";
        return false;
      }

      var newEvent = new Event
      {
        Name = eventDTO.Name,
        StartDate = eventDTO.StartDate,
        EndDate = eventDTO.EndDate,
        Comission = eventDTO.Comission,
        TeamType = eventDTO.TeamType,
        Instantiable = true
      };

      _eventDAL.Insert(newEvent);
      _eventDAL.SaveChanges();

      return true;
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
        x => x.Name == teamDTO.Name && x.Sport_id == teamDTO.Sport
      }).ToList();

      if (existingTeam.Count != 0)
      {
        errorMessage = $"A team with the name {teamDTO.Name} already exists";
        return false;
      }

      var sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == teamDTO.Sport }).FirstOrDefault();

      var newTeam = new Team
      {
        Name = teamDTO.Name,
        Logo = teamDTO.Logo,
        TeamType = teamDTO.TeamType,
        Sport_id = teamDTO.Sport,
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
          Sport = x.Sport.Id
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

      var teams = _teamDAL.Get(new List<Expression<Func<Team, bool>>> { x => x.Id == matchDTO.FirstTeam || x.Id == matchDTO.SecondTeam })
        .ToList();

      var firstTeam = teams.First(x => x.Id == matchDTO.FirstTeam);
      var secondTeam = teams.First(x => x.Id == matchDTO.SecondTeam);

      var sport = _sportDAL.Get(new List<Expression<Func<Sport, bool>>> { x => x.Id == matchDTO.Sport }).FirstOrDefault();

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
        .Select(x => new MatchDTO
        {
          Id = x.Id,
          Date = x.Date,
          FirstTeam = x.FirstTeam.Id,
          FirstTeamScore= x.FirstTeamScore,
          SecondTeam = x.SecondTeam.Id,
          SecondTeamScore = x.SecondTeamScore
        }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
  }
}
