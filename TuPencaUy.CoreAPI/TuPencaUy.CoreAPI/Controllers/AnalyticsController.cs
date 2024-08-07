﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AnalyticsController : BaseController
  {
    private readonly IServiceFactory _serviceFactory;
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IServiceFactory serviceFactory)
    {
      _serviceFactory = serviceFactory;
      _analyticsService = _serviceFactory.GetService<IAnalyticsService>();
    }

    [HttpGet("Leaderboard/{eventId}")]
    public IActionResult GetLeaderboard([Required] int eventId, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
      try
      {
        List<BetUserDTO> leaderboard = _analyticsService.GetLeaderboard(out int count, eventId, page, pageSize);

        return Ok(new ApiResponse
        {
          Data = new
          {
            leaderboard,
            count
          },
          Message = count == 0 ? "No bets found" : "Positions table returned"
        });
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Bets/Matches")]
    public IActionResult GetMatchBets([FromQuery] int? matchId)
    {
      try
      {
        var matchBets = _analyticsService.GetMatchBets(matchId);

        return Ok(new ApiResponse
        {
          Data = matchBets.Count > 1 ? matchBets : matchBets.FirstOrDefault(),
          Message = "Match data returned",
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Bets/Events")]
    public IActionResult GetEventBets([FromQuery] int? eventId)
    {
      try
      {
        var eventBets = _analyticsService.GetEventBets(eventId);

        return Ok(new ApiResponse
        {
          Data = eventBets.Count > 1 ? eventBets : eventBets.FirstOrDefault(),
          Message = "Event data returned",
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Platform/Finances")]
    public IActionResult GetFinances()
    {
      try
      {
        var finances = _analyticsService.GetFinances();

        return Ok(new ApiResponse
        {
          Data = finances,
          Message = "Finances returned"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Platform/Sites")]
    public IActionResult GetSites()
    {
      try
      {
        var sites = _analyticsService.GetSitesAnalytics();

        return Ok(new ApiResponse
        {
          Data = new
          {
            Sites = sites,
            TotalUsers = sites.Sum(x => x.TotalUsers)
          },
          Message = "Sites analytics returned"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Platform/Events")]
    public IActionResult GetEvents()
    {
      try
      {
        var events = _analyticsService.GetEventsAnalytics();

        return Ok(new ApiResponse
        {
          Data = new
          {
            Events = events,
            TotalUsers = events.Sum(x => x.TotalUsers)
          },
          Message = "Sites analytics returned"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
