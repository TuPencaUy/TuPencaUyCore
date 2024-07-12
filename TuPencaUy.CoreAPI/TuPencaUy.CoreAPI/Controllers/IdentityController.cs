﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DataServices.Services.CommonLogic;
using TuPencaUy.CoreAPI.Controllers.Base;
using TuPencaUy.Exceptions;

namespace TuPencaUy.CoreAPI.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class IdentityController : BaseController
  {
    private readonly IAuthLogic _authLogic;
    private readonly IAuthService _authService;
    public IdentityController(IServiceFactory serviceFactory)
    {
      _authService = serviceFactory.GetService<IAuthService>();
      _authLogic = serviceFactory.GetService<IAuthLogic>();
    }

    [HttpPost("BasicLogin")]
    public IActionResult BasicLogin([FromBody] LoginRequest login, bool? withAuth)
    {
      try
      {
        if (string.IsNullOrEmpty(ObtainTenantFromToken())) withAuth = null;

        var user = _authService.Authenticate(login.Email, login.Password, withAuth);
        if (user != null)
        {
          Request.Headers.TryGetValue("currentTenant", out var currentTenant);
          var tokenTuple = _authLogic.GenerateToken(user, currentTenant);
          var token = tokenTuple.Item1;
          var expiration = tokenTuple.Item2;

          var successResponse = new ApiResponse
          {
            Message = $"Welcome {user.Name}",
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
      catch(Exception ex)
      {
        return ManageException(ex);
      }
    }

    [HttpPost("BasicSignup")]
    public IActionResult BasicSignup([FromBody] SignUpRequest request, bool? withAuth)
    {
      if (string.IsNullOrEmpty(ObtainTenantFromToken())) withAuth = null;
      var user = _authService.SignUp(request.Email, request.Password, request.Name, withAuth);

      if (user != null)
      {
        Request.Headers.TryGetValue("currentTenant", out var currentTenant);
        var tokenTuple = _authLogic.GenerateToken(user, currentTenant);
        var token = tokenTuple.Item1;
        var expiration = tokenTuple.Item2;

        var successResponse = new ApiResponse
        {
          Message = $"Welcome {user.Name}",
          Data = new { token, expiration, user },
        };

        return StatusCode((int)HttpStatusCode.Created, successResponse);
      }

      var errorResponse = new ApiResponse
      {
        Error = true,
        Message = $"The email {request.Email} is in use",
      };

      return StatusCode((int)HttpStatusCode.Conflict, errorResponse);
    }

    [HttpPost("OAuthLogin")]
    public IActionResult OAuthLogin([FromBody] string authToken, bool? withAuth)
    {
      if (string.IsNullOrEmpty(ObtainTenantFromToken())) withAuth = null;
      var user = _authService.Authenticate(authToken, withAuth);

      if (user != null)
      {
        Request.Headers.TryGetValue("currentTenant", out var currentTenant);
        var tokenTuple = _authLogic.GenerateToken(user, currentTenant);
        var token = tokenTuple.Item1;
        var expiration = tokenTuple.Item2;

        var successResponse = new ApiResponse
        {
          Message = $"Welcome {user.Name}",
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
