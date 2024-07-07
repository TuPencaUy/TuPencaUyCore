using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("Bets/Users")]
    public IActionResult GetBetsUsers([FromQuery] int? matchId, [FromQuery] int? eventId)
    {
      try
      {
        return Ok();
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
        return Ok();
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
