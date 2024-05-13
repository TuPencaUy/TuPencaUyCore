using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.API.Model;
using TuPencaUy.Core.API.Model.Requests;
using TuPencaUy.Core.API.Model.Responses;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.API.Controllers
{
  [Route("[controller]")]
  public class SiteController : Controller
  {
    private readonly ISiteService _siteService;

    public SiteController(IServiceFactory serviceFactory) => _siteService = serviceFactory.GetService<ISiteService>();

    [Authorize]
    [HttpPost("CreateSite")]
    public IActionResult CreateSite(SiteRequest site)
    {
      var siteDTO = new SiteDTO { Name = site.Name, AccessType = site.AccessType, Color = site.Color, Domain = site.Domain };
      if (!_siteService.CreateNewSite(siteDTO, out string? errorMessage)) return BadRequest(new ApiResponse { Error = true, Message = errorMessage });

      return StatusCode((int)HttpStatusCode.Created ,new ApiResponse { Message = "Successfully created site" });
    }
  }
}
