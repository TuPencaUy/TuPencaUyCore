﻿using Microsoft.AspNetCore.Authorization;
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
    private readonly IEventService _eventService;

    public EventController(IServiceFactory serviceFactory)
    {
      _eventService = serviceFactory.GetService<IEventService>();
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
        Sport = new SportDTO { Id = match.Sport, Tie = false, Name = string.Empty },
        FirstTeamScore = match.FirstTeamScore,
        SecondTeamScore = match.SecondTeamScore,
        Date = match.Date,
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
        var m =_eventService.ModifyMatch(idMatch, match.FirstTeam, match.SecondTeam, match.Date, match.FirstTeamScore, match.SecondTeamScore, match.Sport);

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
        var t =_eventService.ModifyTeam(idTeam, team.Name, team.Logo, team.TeamType, team.Sport);

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
        var e = _eventService.ModifyEvent(idEvent, ev.Name, ev.StartDate, ev.EndDate, ev.Comission, ev.TeamType);

        var successResponse = new ApiResponse
        {
          Data= e,
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
