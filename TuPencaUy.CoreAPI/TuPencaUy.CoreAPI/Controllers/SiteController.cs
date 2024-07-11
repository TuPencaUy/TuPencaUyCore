using Microsoft.AspNetCore.Authorization;
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
    private readonly IUserService _userService;

    public SiteController(IServiceFactory serviceFactory)
    {
      _siteService = serviceFactory.GetService<ISiteService>();
      _userService = serviceFactory.GetService<IUserService>();
      _serviceFactory = serviceFactory;
    }

    [HttpGet]
    public IActionResult GetSites()
    {
      try
      {
        return Ok(new ApiResponse { Data = _siteService.GetSites()});
      }
      catch (SiteNotFoundException)
      {
        return NotFound(new ApiResponse { Error = true, Message = "Something was wrong" });
      }
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
      try
      {
        if (site.Name.Contains(' ')) throw new InvalidNameOfSiteException("The site name can't contain withespaces");


        var siteDTO = new SiteDTO { Name = site.Name, AccessType = site.AccessType, Color = site.Color, Domain = site.Domain };

        UserDTO userFromToken = ObtainUserFromToken();

        var created = _siteService
          .CreateNewSite(userFromToken.Email, siteDTO, out string? errorMessage, out string? connectionString);

        if (!created) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

        userFromToken = _userService.GetUserByEmail(userFromToken.Email);

        _serviceFactory
          .CreateTenantServices(connectionString);
        _serviceFactory
          .GetService<IUserService>()
          .CreateUser(userFromToken.Email, userFromToken.Name, userFromToken.Password, UserRoleEnum.Admin);

        return StatusCode((int)HttpStatusCode.Created, new ApiResponse { Message = "Successfully created site" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
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
        return ManageException(ex);
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
          Id = siteID,
          PaypalEmail = site.PaypalEmail
        };

        _siteService.UpdateSite(siteDTO);

        return Ok(new ApiResponse { Message = "Successfully updated site" });
      }
      catch (Exception ex)
      {
        return ManageException(ex);
      }
    }
  }
}
