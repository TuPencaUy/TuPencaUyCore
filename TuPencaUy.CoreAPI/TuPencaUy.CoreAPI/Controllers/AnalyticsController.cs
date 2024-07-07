using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.DataServices;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AnalyticsController : BaseController
  {
    private readonly IServiceFactory _serviceFactory;

    public AnalyticsController(IServiceFactory serviceFactory)
    {
      _serviceFactory = serviceFactory;
    }

    [HttpGet("PositionsTable/{eventId}")]
    public IActionResult GetPositionsTable([Required] int eventId, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
      try
      {
        return Ok();
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
        return Ok();
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
