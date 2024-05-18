using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using TuPencaUy.Core.DAO;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformEventService : IEventService
  {
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IGenericRepository<Sport> _sportDAL;
    private readonly IGenericRepository<Team> _teamDAL;
    private readonly IGenericRepository<Match> _matchDAL;
    public PlatformEventService(IGenericRepository<Event> eventDAL, IGenericRepository<Sport> sportDAL, IGenericRepository<Team> teamDAL, IGenericRepository<Match> matchDAL)
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
        TeamType = eventDTO.TeamType
      };
      _eventDAL.Insert(newEvent);
      _eventDAL.SaveChanges();

      return true;
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

    public bool CreateTeam(TeamDTO teamDTO, out string? errorMessage)
    {
      errorMessage = null;
      var existingTeam = _teamDAL.Get(new List<Expression<Func<Team, bool>>>
      {
        x => x.Name == teamDTO.Name
      }).ToList();

      if (existingTeam.Count != 0)
      {
        errorMessage = $"A team with the name {teamDTO.Name} already exists";
        return false;
      }

      var newTeam = new Team
      {
        Name = teamDTO.Name,
        Logo = teamDTO.Logo,
        TeamType = teamDTO.TeamType,
      };
      _teamDAL.Insert(newTeam);
      _teamDAL.SaveChanges();

      return true;
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
      var newMatch = new Match
      {
       FirstTeam = matchDTO.FirstTeam,
       SecondTeam = matchDTO.SecondTeam,
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
  }
}
