using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.ResultsAPI.Controllers.Base;
using TuPencaUy.ResultsAPI.Model;

namespace TuPencaUy.ResultsAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ResultController(IEventService eventService) : BaseController
  {
    private readonly IEventService _eventService = eventService;

    [HttpGet("Match/{id}")]
    public IActionResult GetMatchResult([Required] int id)
    {
      try
      {
        var m = _eventService.GetMatch(id);
        return Ok(new ApiResponse
        {
          Data = new
          {
            m.Id,
            m.FirstTeamScore,
            m.SecondTeamScore,
            GameStatusCode = m.Finished.Value ? GameStatusEnum.Finalized : CalculateGameStatus(m.Date),
            GameStatus = Enum.GetName(typeof(GameStatusEnum), m.Finished.Value ? GameStatusEnum.Finalized : CalculateGameStatus(m.Date))
          },
          Message = "Match result returned"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet("Event/{eventId}")]
    public IActionResult GetMatchesResult([Required] int eventId)
    {
      try
      {
        var m = _eventService.GetMatches(out int count, null, null, eventId, null, null, null, null, int.MaxValue, int.MaxValue);
        var matchesResult = m.Select(x => new
        {
          x.Id,
          x.FirstTeamScore,
          x.SecondTeamScore,
          GameStatusCode = x.Finished.Value ? GameStatusEnum.Finalized : CalculateGameStatus(x.Date),
          GameStatus = Enum.GetName(typeof(GameStatusEnum), x.Finished.Value ? GameStatusEnum.Finalized : CalculateGameStatus(x.Date))
        }).ToArray();

        return Ok(new ApiResponse
        {
          Data = new { matches = matchesResult, count = m.Count },
          Message = "Matches result returned"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
