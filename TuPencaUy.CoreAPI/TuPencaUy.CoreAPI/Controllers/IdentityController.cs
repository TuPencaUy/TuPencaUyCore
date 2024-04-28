using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
    public IdentityController(IAuthService authService)
    {
      _authService = authService; 
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequestDTO login)
    {
      var user = _authService.Authenticate(login);

      if (user != null)
      {
        var token = _authService.GenerateToken(user);

        return Ok(token);
      }

      return NotFound("UserNotFound");
    }
  }
}
