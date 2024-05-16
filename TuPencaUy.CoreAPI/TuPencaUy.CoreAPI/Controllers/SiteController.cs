using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
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

    [Authorize]
    [HttpPost("CreateSite")]
    public IActionResult CreateSite([FromBody] SiteRequest site)
    {
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

      return StatusCode((int)HttpStatusCode.Created ,new ApiResponse { Message = "Successfully created site" });
    }
  }
}
