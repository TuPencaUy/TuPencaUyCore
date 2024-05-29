using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.CoreAPI.Controllers.Base;
using TuPencaUy.Exceptions;

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

    [HttpGet("{userId}")]
    [Authorize]
    public IActionResult Get(int userId)
    {
      try
      {
        var user = _userService.GetUserById(userId);

        var response = new ApiResponse
        {
          Data = user,
          Message = $"Users with id {userId} was found"
        };

        return StatusCode((int)HttpStatusCode.OK, response);

      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpGet]
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
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
