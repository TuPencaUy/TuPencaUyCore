﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.CoreAPI.Controllers.Base;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class SiteController : BaseController
  {
    private readonly IServiceFactory _serviceFactory;
    private readonly ISiteService _siteService;

    public SiteController(IServiceFactory serviceFactory)
    {
      _siteService = serviceFactory.GetService<ISiteService>();
      _serviceFactory = serviceFactory;
    }

    [HttpGet("{domain}")]
    public IActionResult GetSite(string domain)
    {
      try
      {
        return Ok(new ApiResponse { Data = _siteService.GetSiteByDomain(domain) });
      }
      catch (SiteNotFoundException)
      {
        return NotFound(new ApiResponse { Error = true, Message = "Site not found" });
      }
    }

    [Authorize]
    [HttpPost("CreateSite")]
    public IActionResult CreateSite([FromBody] SiteRequest site)
    {
      if (site.Name.Contains(' '))
      {
        var response = new ApiResponse
        {
          Error = true,
          Message = "The site name can't contain withespaces"
        };

        return BadRequest(response);
      }

      var siteDTO = new SiteDTO { Name = site.Name, AccessType = site.AccessType, Color = site.Color, Domain = site.Domain };

      UserDTO userFromToken = ObtainUserFromToken();

      var created = _siteService
        .CreateNewSite(userFromToken.Email, siteDTO, out string? errorMessage, out string? connectionString);

      if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      _serviceFactory
        .CreateTenantServices(connectionString);
      _serviceFactory
        .GetService<IUserService>()
        .CreateUser(userFromToken.Email, userFromToken.Name, UserRoleEnum.Admin);

      return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created site" });
    }


    [Authorize]
    [HttpDelete("{siteID}")]
    public IActionResult DeleteSite(int siteID)
    {
      try
      {
        _siteService.DeleteSite(siteID);

        return Ok(new ApiResponse { Message = "Successfully deleted site" });
      }
      catch (SiteNotFoundException)
      {
        return NotFound(new ApiResponse { Error = true, Message = "Site not found" });
      }
      catch (Exception ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { Message = "Internal Error" });
      }
    }


    [Authorize]
    [HttpPut("{siteID}")]
    public IActionResult UpdateSite(int siteID, [FromBody] SiteRequest site)
    {
      try
      {
        SiteDTO siteDTO = new SiteDTO()
        {
          AccessType = site.AccessType,
          Color = site.Color,
          Domain = site.Domain,
          Name = site.Name,
          Id = siteID
        };

        _siteService.UpdateSite(siteDTO);

        return Ok(new ApiResponse { Message = "Successfully updated site" });
      }
      catch (SiteNotFoundException)
      {
        return NotFound(new ApiResponse { Error = true, Message = "Site not found" });
      }
      catch (Exception ex)
      {
        return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { Message = "Internal Error" });
      }
    }
  }
}
