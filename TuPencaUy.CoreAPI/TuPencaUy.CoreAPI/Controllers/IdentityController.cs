using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model;
using TuPencaUy.Core.DataServices;
using TuPencaUy.CoreAPI.Controllers.Base;
using TuPencaUy.DTOs;
using TuPencaUy.Platform.DataServices.Services;

namespace TuPencaUy.CoreAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IdentityController : BaseController
  {
    private readonly IAuthService _authService;
    public IdentityController(IServiceFactory serviceFactory)
    {
      _authService = serviceFactory.GetService<IAuthService>(); 
    }

    [HttpPost("BasicLogin")]
    public IActionResult BasicLogin([FromBody] LoginRequestDTO login)
    {
      var user = _authService.Authenticate(login);

      if (user != null)
      {
        var tokenTuple = _authService.GenerateToken(user);
        var token = tokenTuple.Item1;
        var expiration = tokenTuple.Item2;

        var successResponse = new ApiResponse
        {
          Message = $"Wellcome {user.Name}",
          Data = new { token, expiration, user },
        };

        return Ok(successResponse);
      }

      var errorResponse = new ApiResponse
      {
        Error = true,
        Message = "User not found",
      };

      return StatusCode((int)HttpStatusCode.NotFound, errorResponse);
    }

    [HttpPost("OAuthLogin")]
    public IActionResult OAuthLogin([FromBody] string authToken)
    {
      var user = _authService.Authenticate(authToken);

      if (user != null)
      {
        var tokenTuple = _authService.GenerateToken(user);
        var token = tokenTuple.Item1;
        var expiration = tokenTuple.Item2;

        var successResponse = new ApiResponse
        {
          Message = $"Wellcome {user.Name}",
          Data = new { token, expiration, user },
        };

        return Ok(successResponse);
      }

      var errorResponse = new ApiResponse
      {
        Error = true,
        Message = "User not found",
      };
      return StatusCode((int)HttpStatusCode.NotFound, errorResponse);
    }
  }
}
