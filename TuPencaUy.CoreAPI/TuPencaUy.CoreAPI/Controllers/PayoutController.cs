using Microsoft.AspNetCore.Mvc;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DataServices;
using TuPencaUy.CoreAPI.Controllers.Base;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class PayoutController : BaseController
  {
    private readonly IPayoutService _payoutService;
    public PayoutController(IServiceFactory serviceFactory) => _payoutService = serviceFactory.GetService<IPayoutService>();

    [HttpPost]
    public IActionResult Create([FromBody] CreatePayoutRequest payoutRequest)
    {
      try
      {
        var payout = _payoutService.CreatePayout(
          payoutRequest.PaypalEmail,
          payoutRequest.EventId,
          payoutRequest.SiteId,
          payoutRequest.Amount,
          payoutRequest.TransactionID);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = payout, Message = "Payout created" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet]
    public IActionResult Get(string? userEmail, int? eventId, string? transactionId, int? siteId, int? page, int? pageSize)
    {
      try
      {
        var list = _payoutService.GetPayouts(out int count, userEmail, eventId, transactionId, siteId, page, pageSize);

        return Ok(new ApiResponse { Data = new { list, count }, Message = "Returned payouts" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
