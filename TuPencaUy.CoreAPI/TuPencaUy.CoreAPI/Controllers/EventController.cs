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

      return StatusCode((int)HttpStatusCode.Created ,new ApiResponse { Message = "Successfully created event" });
    }

    [HttpGet]
    public IActionResult GetEvents()
    {
      try
      {
        var list = _eventService.GetEvents();
        var successResponse = new ApiResponse
        {
          Data = list,
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

    [HttpPost("Sport")]
    public IActionResult CreateSport([FromBody] CreateSportRequest sport)
    {
      var sportDTO = new SportDTO
      {
        Name = sport.Name,
        Tie= sport.Tie,
        ExactPoints= sport.ExactPoints,
        PartialPoints = sport.PartialPoints
      };

      var created = _eventService.CreateSport(sportDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created sport" });
    }

    [HttpGet("Sport")]
    public IActionResult GetSport()
    {
      try
      {
        var list = _eventService.GetSports();
        var successResponse = new ApiResponse
        {
          Data = list,
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

    [HttpPost("Team")]
    public IActionResult CreateTeam([FromBody] CreateTeamRequest team)
    {
      var teamDTO = new TeamDTO
      {
        Name = team.Name,
        Logo = team.Logo,
      };

      var created = _eventService.CreateTeam(teamDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created team" });
    }

    [HttpGet("Team")]
    public IActionResult GetTeams()
    {
      try
      {
        var list = _eventService.GetTeams();
        var successResponse = new ApiResponse
        {
          Data = list,
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

    [HttpPost("Match")]
    public IActionResult CreateMatch([FromBody] CreateMatchRequest match)
    {
      var matchDTO = new MatchDTO
      {
        FirstTeam = match.FirstTeam,
        SecondTeam = match.SecondTeam,
        FirstTeamScore = match.FirstTeamScore,
        SecondTeamScore = match.SecondTeamScore,
        Date = match.Date
      };

      var created = _eventService.CreateMatch(match.EventId, matchDTO, out string? errorMessage);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created match" });
    }

    [HttpGet("Match")]
    public IActionResult GetMatches()
    {
      try
      {
        var list = _eventService.GetMatches();
        var successResponse = new ApiResponse
        {
          Data = list,
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
  }
}
