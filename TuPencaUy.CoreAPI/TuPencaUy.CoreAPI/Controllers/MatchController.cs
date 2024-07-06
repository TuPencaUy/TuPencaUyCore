using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MatchController : BaseController
  {
    private readonly IEventService _eventService;
    private readonly IServiceFactory _serviceFactory;

    public MatchController(IServiceFactory serviceFactory)
    {
      _eventService = serviceFactory.GetService<IEventService>();
      _serviceFactory = serviceFactory;
    }

    [HttpPost]
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

        ManageTenantMatches(idEvent: match.EventId, createdMatch);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = createdMatch, Message = "Successfully created match" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet]
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

    [HttpGet("{idMatch}")]
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

    [HttpPatch("{idMatch}")]
    public IActionResult ModifyMatch(int idMatch, [FromBody] ModifyMatchRequest match)
    {
      try
      {
        if (!string.IsNullOrEmpty(ObtainTenantFromToken()))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a central platform" });
        }

        var m = _eventService.ModifyMatch(idMatch, match.FirstTeam, match.SecondTeam, match.Date, match.FirstTeamScore, match.SecondTeamScore, match.Sport);

        ManageTenantMatches(idEvent: null, m);

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

    [HttpDelete("{idMatch}")]
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

    private void ManageTenantMatches(int? idEvent, MatchDTO match)
    {
      var sites = _serviceFactory.GetService<ISiteService>().GetSites();

      foreach(var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);

        if (idEvent.HasValue) CreateTenantMatches(idEvent.Value, match);
        else ModifyTenantMatches(match);
      }
    }

    private void CreateTenantMatches(int idEvent, MatchDTO match)
    {
      var localEventService = _serviceFactory.GetService<IEventService>();

      var tenantEvents = localEventService.GetEvents(refEventId: idEvent);

      if (tenantEvents.Any())
      {
        foreach(var tenantEvent in tenantEvents)
        {
          localEventService.CreateMatch(
            tenantEvent.Id.Value,
            match.FirstTeam.Id,
            match.SecondTeam.Id,
            match.FirstTeamScore,
            match.SecondTeamScore,
            match.Sport.Id.Value,
            match.Date.Value,
            match.Id);
        }
      }
    }
    private void ModifyTenantMatches(MatchDTO match)
    {
      var localEventService = _serviceFactory.GetService<IEventService>();

      localEventService.ModifyMatches(
        match.FirstTeam.Id,
        match.SecondTeam.Id,
        match.Date,
        match.FirstTeamScore,
        match.SecondTeamScore,
        match.Sport.Id.Value,
        match.Id
        );
    }
  }
}
