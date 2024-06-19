using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.CoreAPI.Controllers.Base;

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
        var bets = _betService.GetBets(out int count, userEmail, matchId, eventId, page, pageSize);

        return Ok(new ApiResponse { Message = "Returned bets" });
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
