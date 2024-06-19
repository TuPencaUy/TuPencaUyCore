using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.Enums;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccessRequestController(IAccessRequestService accessRequestService) : BaseController
  {
    private readonly IAccessRequestService _accessRequestService = accessRequestService;

    [HttpGet]
    [Authorize]
    public IActionResult Get(int? page, int? pageSize, AccessStatusEnum? accessStatusEnum, string? email)
    {
      try
      {
        if (string.IsNullOrEmpty(ObtainTenantFromToken())) return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a tenant" });

        var requests = _accessRequestService.GetAccessRequests(out int count, email, accessStatusEnum, page, pageSize);
        return Ok(new ApiResponse { Data = new {requests, count}, Message = "Requests founded"});
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPatch]
    [Authorize]
    public IActionResult Modify([Required] AccessStatusEnum accessStatusEnum, [Required] string email)
    {
      try
      {
        if (string.IsNullOrEmpty(ObtainTenantFromToken())) return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a tenant" });

        var request = _accessRequestService.ChangeState(accessStatusEnum, email);
        return Ok(new ApiResponse { Data = request, Message = "Requests founded" });
      }
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
