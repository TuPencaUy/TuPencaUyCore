﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class BetController : BaseController
  {
    private readonly IBetService _betService;
    private readonly IServiceFactory _serviceFactory;
    public BetController(IServiceFactory serviceFactory)
    {
      _serviceFactory = serviceFactory;
      _betService = serviceFactory.GetService<IBetService>();
    }


    [HttpPost("EndEvent/{idEvent}")]
    public IActionResult EndEvent([Required] int idEvent)
    {
      try
      {
        var tenant = ObtainTenantFromToken();
        if (string.IsNullOrEmpty(tenant))
        {
          return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a tenant" });
        }

        var payment = _betService.EndEvent(idEvent);

        _serviceFactory.CreatePlatformServices();
        var site = _serviceFactory.GetService<ISiteService>().GetSiteByDomain(tenant);

        payment.SitePaypalEmail = site.PaypalEmail;

        return Ok(new ApiResponse
        {
          Data = payment,
          Message = "Event ended"
        });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

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

    [HttpPatch]
    public IActionResult ModifyBet([Required] ModifyBetRequest request)
    {
      try
      {
        var bet = _betService.ModifyBet(request.UserEmail, request.MatchId, request.EventId, request.FirstTeamScore, request.SecondTeamScore);

        return Ok(new ApiResponse { Data = bet, Message = "Modified bet" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch("UpdatePoints")]
    public IActionResult UpdatePoints([Required] string userEmail, [Required] int matchId, [Required] int eventId)
    {
      try
      {
        _betService.UpdatePoints(userEmail, matchId, eventId);

        return Ok(new ApiResponse { Message = "Points updated"});
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
