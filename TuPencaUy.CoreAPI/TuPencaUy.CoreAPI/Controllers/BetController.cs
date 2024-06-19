using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.CoreAPI.Controllers.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TuPencaUy.Core.API.Controllers
{
  public class BetController : BaseController
  {
    private readonly IBetService _betService;
    public BetController(IServiceFactory serviceFactory) => _betService = serviceFactory.GetService<IBetService>();

    [HttpGet]
    public IActionResult Get(int? page, int? pageSize, string? userEmail, int? matchId, int? eventId)
    {
      try
      {
        var list = _betService.GetBets(out int count, userEmail, matchId, eventId, page, pageSize);

        return Ok(new ApiResponse { Data = new { list, count }, Message = "Returned bets" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpDelete]
    public IActionResult DeleteBet([Required] string userEmail, [Required] int matchId, [Required] int eventId)
    {
      try
      {
        _betService.DeleteBet(userEmail, matchId, eventId);
        return Ok(new ApiResponse { Message = "Bet successfully deleted" });
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost]
    public IActionResult CreateBet([FromBody] CreateBetRequest betRequest)
    {
      try
      {
        var bet = _betService.CreateBet(
          betRequest.UserEmail,
          betRequest.MatchId,
          betRequest.EventId,
          betRequest.FirstTeamScore,
          betRequest.SecondTeamScore);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = bet, Message = "Bet created"});
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
