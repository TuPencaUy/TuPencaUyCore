﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;

namespace TuPencaUy.CoreAPI.Controllers.Base
{
  public class BaseController : Controller
  {
    protected UserDTO ObtainUserFromToken()
    {
      string authHeader = Request.Headers["Authorization"];

      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadJwtToken(authHeader.Split(' ')[1].Trim());
      var email = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
      var name = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

      return new UserDTO { Name = name, Email = email };
    }

    protected string ObtainTenantFromToken()
    {
      string authHeader = Request.Headers["Authorization"];

      var tokenHandler = new JwtSecurityTokenHandler();
      var jwtToken = tokenHandler.ReadJwtToken(authHeader.Split(' ')[1].Trim());
      var currentTenant = jwtToken.Claims.FirstOrDefault(x => x.Type == "currentTenant")?.Value;

      return currentTenant;
    }

    protected IActionResult ManageException(Exception ex)
    {
      var errorResponse = new ApiResponse();

      if (ex is NotFoundException)
      {
        errorResponse.Error = true;
        errorResponse.Message = ex.Message;
        return StatusCode((int)HttpStatusCode.NotFound, errorResponse);
      }

      if(ex is BadRequestException)
      {
        errorResponse.Error = true;
        errorResponse.Message = ex.Message;
        return StatusCode((int)HttpStatusCode.BadRequest, errorResponse);
      }

      errorResponse.Error = true;
      errorResponse.Message = "An internal error has occurred";
      return StatusCode((int)HttpStatusCode.InternalServerError, errorResponse);
    }
  }
}
