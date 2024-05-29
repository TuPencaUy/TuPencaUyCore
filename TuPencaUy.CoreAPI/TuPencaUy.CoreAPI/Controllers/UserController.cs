using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.CoreAPI.Controllers.Base;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class UserController : BaseController
  {
    private readonly IUserService _userService;
    public UserController(IServiceFactory serviceFactory)
    {
      _userService = serviceFactory.GetService<IUserService>();
    }

    [HttpGet("{eventId}")]
    [Authorize]
    public IActionResult GetByEvent(int eventId)
    {
      if (string.IsNullOrEmpty(ObtainTenantFromToken()))
      {
        return BadRequest(new ApiResponse { Error = true, Message = "You must be logged to a tenant" });
      }

      try
      {
        var users = _userService.GetUsersByEvent(eventId);

        var response = new ApiResponse
        {
          Data = users,
          Message = users.Any() ? "Users found" : "The event has no users",
        };

        return StatusCode((int)HttpStatusCode.OK, response);
      }
      catch(EventNotFoundException ex)
      {
        var errorResponse = new ApiResponse
        {
          Error = true,
          Message = ex.Message,
        };

        return StatusCode((int)HttpStatusCode.NotFound, errorResponse);
      }
      catch (Exception)
      {
        var errorResponse = new ApiResponse
        {
          Error = true,
          Message = "An internal error has occurred",
        };

        return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
      }
    }
  }
}
