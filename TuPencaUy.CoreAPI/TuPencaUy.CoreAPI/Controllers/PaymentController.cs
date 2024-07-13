using Microsoft.AspNetCore.Mvc;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DataServices;
using TuPencaUy.CoreAPI.Controllers.Base;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class PaymentController : BaseController
  {
    private readonly IPaymentService _paymentService;
    private readonly IServiceFactory _serviceFactory;

    public PaymentController(IServiceFactory serviceFactory){
      _paymentService = serviceFactory.GetService<IPaymentService>();
      _serviceFactory = serviceFactory;
     }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePaymentRequest paymentRequest)
    {
      try
      {
        var payment = _paymentService.CreatePayment(
          paymentRequest.UserEmail,
          paymentRequest.EventId,
          paymentRequest.Amount,
          paymentRequest.TransactionID);

        _serviceFactory.CreatePlatformServices();
        ISiteService siteService = _serviceFactory.GetService<ISiteService>();
        int siteId = siteService.GetSiteByDomain(ObtainTenantFromToken()).Id;

        _serviceFactory.GetService<IPaymentService>().CreatePayment(
          paymentRequest.UserEmail,
          payment.Event.ReferenceEvent.Value,
          paymentRequest.Amount,
          paymentRequest.TransactionID,
          siteId);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Data = payment, Message = "Payment created" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet]
    public IActionResult Get(string? userEmail, int? eventId, string? transactionId, int? page, int? pageSize)
    {
      try
      {
        var list = _paymentService.GetPayments(out int count, userEmail, eventId, transactionId, page, pageSize);

        return Ok(new ApiResponse { Data = new { list, count }, Message = "Returned payments" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch]
    public IActionResult Modify([Required] ModifyPaymentRequest request)
    {
      try
      {
        var payment = _paymentService.ModifyPayment(request.UserEmail, request.EventId, request.amount, request.transactionId);

        return Ok(new ApiResponse { Data = payment, Message = "Modified payment" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpDelete("{paymentId}")]
    public IActionResult Delete(int paymentId)
    {
      try
      {
        _paymentService.DeletePayment(paymentId);
        return Ok(new ApiResponse { Message = "Payment successfully deleted" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
