using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
    private readonly IEventService _eventService;
    private readonly IServiceFactory _serviceFactory;

    public EventController(IServiceFactory serviceFactory)
    {
      _eventService = serviceFactory.GetService<IEventService>();
      _serviceFactory = serviceFactory;
    }

    [HttpPost("InstantiateEvent")]
    public IActionResult InstantiateEvent([Required] int eventId)
    {
      try
      {
        string domain = ObtainTenantFromToken();
        if (string.IsNullOrEmpty(domain))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a tenant" });
        }

        _serviceFactory.CreatePlatformServices();
        var platformEventService = _serviceFactory.GetService<IEventService>();

        var searchedEvent = platformEventService.GetEvent(eventId);
        var searchedMatches = platformEventService.GetMatches(
          out int count,
          idTeam: null,
          otherIdTeam: null,
          eventId: eventId,
          sportId: null,
          fromDate: null,
          untilDate: null,
          page: null,
          pageSize: int.MaxValue);

        string connStringTennant = _serviceFactory.GetService<ISiteService>().GetSiteByDomain(domain).ConnectionString;

        _serviceFactory.CreateTenantServices(connStringTennant);

        var newEvent = _serviceFactory.GetService<IEventService>().InstantiateEvent(searchedEvent, searchedMatches);

        var data = new { Event = newEvent.Item1, Matches = newEvent.Item2 };

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = data, Message = "Successfully instantiated event" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost]
    public IActionResult CreateEvent([FromBody] CreateEventRequest requestEvent)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var createdEvent = _eventService
          .CreateEvent(requestEvent.Name, requestEvent.StartDate, requestEvent.EndDate, requestEvent.Comission, requestEvent.TeamType, requestEvent.Sport_id);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = createdEvent, Message = "Successfully created event" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet]
    public IActionResult GetEvents(int? page, int? pageSize, string? name, DateTime? fromDate, DateTime? untilDate, TeamTypeEnum? teamType, bool? instantiable)
    {
      try
      {
        var list = _eventService.GetEvents(
          out int count,
          name,
          fromDate,
          untilDate,
          teamType,
          instantiable,
          page, pageSize);

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
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var createdSport = _eventService.CreateSport(sport.Name, sport.Tie, sport.ExactPoints, sport.PartialPoints);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = createdSport, Message = "Successfully created sport" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Sport")]
    public IActionResult GetSport(int? page, int? pageSize, string? name)
    {
      try
      {
        var list = _eventService.GetSports(out int count, name, page, pageSize);

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
      if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
      {
        return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
      }

      try
      {
        byte[]? logo = team.Logo == null ? null : Convert.FromBase64String(team.Logo);
        var createdTeam = _eventService.CreateTeam(team.Name, logo, team.Sport, team.TeamType);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = createdTeam, Message = "Successfully created team" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Team")]
    public IActionResult GetTeams(int? page, int? pageSize, string? name, int? sportId, TeamTypeEnum? teamType)
    {
      try
      {
        var list = _eventService.GetTeams(out int count, name, sportId, teamType, page, pageSize);

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
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var createdMatch = _eventService
          .CreateMatch(match.EventId, match.FirstTeam, match.SecondTeam, match.FirstTeamScore, match.SecondTeamScore, match.Sport, match.Date);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = createdMatch, Message = "Successfully created match" });
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Match")]
    public IActionResult GetMatches(int? page, int? pageSize, int? idTeam, int? otherIdTeam, int? eventId, int? sportId, DateTime? fromDate, DateTime? untilDate)
    {
      try
      {
        var list = _eventService.GetMatches(
          out int count,
          idTeam,
          otherIdTeam,
          eventId,
          sportId,
          fromDate,
          untilDate,
          page, pageSize);

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

    [HttpGet("{idEvent}")]
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

    [HttpDelete("Match/{idMatch}")]
    public IActionResult DeleteMatch(int idMatch)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        _eventService.DeleteMatch(idMatch);

        var successResponse = new ApiResponse
        {
          Message = $"The match with id {idMatch} was deleted",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpDelete("Team/{idTeam}")]
    public IActionResult DeleteTeam(int idTeam)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        _eventService.DeleteTeam(idTeam);

        var successResponse = new ApiResponse
        {
          Message = $"The team with id {idTeam} was deleted",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpDelete("Sport/{idSport}")]
    public IActionResult DeleteSport(int idSport)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        _eventService.DeleteSport(idSport);

        var successResponse = new ApiResponse
        {
          Message = $"The sport with id {idSport} was deleted",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpDelete("{idEvent}")]
    public IActionResult DeleteEvent(int idEvent)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        _eventService.DeleteEvent(idEvent);

        var successResponse = new ApiResponse
        {
          Message = $"The event with id {idEvent} was deleted",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch("Match/{idMatch}")]
    public IActionResult ModifyMatch(int idMatch, [FromBody] ModifyMatchRequest match)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var m = _eventService.ModifyMatch(idMatch, match.FirstTeam, match.SecondTeam, match.Date, match.FirstTeamScore, match.SecondTeamScore, match.Sport);

        var successResponse = new ApiResponse
        {
          Data = m,
          Message = $"The match with id {idMatch} was modified",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch("Team/{idTeam}")]
    public IActionResult ModifyTeam(int idTeam, [FromBody] ModifyTeamRequest team)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var t = _eventService.ModifyTeam(idTeam, team.Name, team.Logo, team.TeamType, team.Sport);

        var successResponse = new ApiResponse
        {
          Data = t,
          Message = $"The team with id {idTeam} was modified",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch("Sport/{idSport}")]
    public IActionResult ModifySport(int idSport, [FromBody] ModifySportRequest sport)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var s = _eventService.ModifySport(idSport, sport.Name, sport.Tie, sport.ExactPoints, sport.PartialPoints);

        var successResponse = new ApiResponse
        {
          Data = s,
          Message = $"The sport with id {idSport} was modified",
        };

        return Ok(successResponse);
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch("{idEvent}")]
    public IActionResult ModifyEvent(int idEvent, [FromBody] ModifyEventRequest ev)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var e = _eventService.ModifyEvent(idEvent, ev.Name, ev.StartDate, ev.EndDate, ev.Comission, ev.TeamType, ev.Instantiable);

        var successResponse = new ApiResponse
        {
          Data = e,
          Message = $"The event with id {idEvent} was modified",
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
