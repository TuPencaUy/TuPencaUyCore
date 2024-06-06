using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.CoreAPI.Controllers.Base;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class EventController : BaseController
  {
    private readonly IServiceFactory _serviceFactory;
    private readonly IEventService _eventService;

    public EventController(IServiceFactory serviceFactory)
    {
      _eventService = serviceFactory.GetService<IEventService>();
      _serviceFactory = serviceFactory;
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] CreateEventRequest requestEvent)
    {
      var eventDTO = new EventDTO {
        Name = requestEvent.Name,
        StartDate = requestEvent.StartDate,
        EndDate = requestEvent.EndDate,
        Comission = requestEvent.Comission
      };

      var created = _eventService.CreateEvent(eventDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created event" });
    }

    [HttpGet]
    public IActionResult GetEvents(int page, int pageSize)
    {
      try
      {
        var list = _eventService.GetEvents(page, pageSize, out int count);

        var successResponse = new ApiResponse
        {
          Data = new
          {
            list,
            count
          },
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost("Sport")]
    public IActionResult CreateSport([FromBody] CreateSportRequest sport)
    {
      var sportDTO = new SportDTO
      {
        Name = sport.Name,
        Tie = sport.Tie,
        ExactPoints = sport.ExactPoints,
        PartialPoints = sport.PartialPoints
      };

      var created = _eventService.CreateSport(sportDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created sport" });
    }

    [HttpGet("Sport")]
    public IActionResult GetSport(int page, int pageSize)
    {
      try
      {
        var list = _eventService.GetSports(page, pageSize, out int count);

        var successResponse = new ApiResponse
        {
          Data = new
          {
            list,
            count
          },
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost("Team")]
    public IActionResult CreateTeam([FromBody] CreateTeamRequest team)
    {
      var teamDTO = new TeamDTO
      {
        Name = team.Name,
        Logo = team.Logo,
        // TODO review this or the CreateTeam method to receive sport ID
        Sport = new SportDTO { Id = team.Sport, Name = string.Empty, Tie = false },
        TeamType = team.TeamType,
      };

      var created = _eventService.CreateTeam(teamDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created team" });
    }

    [HttpGet("Team")]
    public IActionResult GetTeams(int page, int pageSize)
    {
      try
      {
        var list = _eventService.GetTeams(page, pageSize, out int count);

        var successResponse = new ApiResponse
        {
          Data = new
          {
            list,
            count
          },
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost("Match")]
    public IActionResult CreateMatch([FromBody] CreateMatchRequest match)
    {
      var matchDTO = new MatchDTO
      {
        // TODO review this or the CreateMatch method to receive team IDs
        FirstTeam =  new TeamDTO { Id = match.FirstTeam, Name = string.Empty },
        SecondTeam = new TeamDTO { Id = match.FirstTeam, Name = string.Empty },
        FirstTeamScore = match.FirstTeamScore,
        SecondTeamScore = match.SecondTeamScore,
        Date = match.Date
      };

      var created = _eventService.CreateMatch(match.EventId, matchDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created match" });
    }

    [HttpGet("Match")]
    public IActionResult GetMatches(int page, int pageSize)
    {
      try
      {
        var list = _eventService.GetMatches(page, pageSize, out int count);

        var successResponse = new ApiResponse
        {
          Data = new
          {
            list,
            count
          },
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        var errorResponse = new ApiResponse
        {
          Message = ex.Message,
          Error = true
        };
        return BadRequest(errorResponse);
      }
    }

    [HttpGet("Match/{idMatch}")]
    public IActionResult GetMatch(int idMatch)
    {
      try
      {
        var match = _eventService.GetMatch(idMatch);

        var successResponse = new ApiResponse
        {
          Data = match,
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Team/{idTeam}")]
    public IActionResult GetTeam(int idTeam)
    {
      try
      {
        var team = _eventService.GetTeam(idTeam);

        var successResponse = new ApiResponse
        {
          Data = team,
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Sport/{idSport}")]
    public IActionResult GetSport(int idSport)
    {
      try
      {
        var sport = _eventService.GetSport(idSport);

        var successResponse = new ApiResponse
        {
          Data = sport,
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Event/{idEvent}")]
    public IActionResult GetEvent(int idEvent)
    {
      try
      {
        var ev = _eventService.GetEvent(idEvent);

        var successResponse = new ApiResponse
        {
          Data = ev,
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
