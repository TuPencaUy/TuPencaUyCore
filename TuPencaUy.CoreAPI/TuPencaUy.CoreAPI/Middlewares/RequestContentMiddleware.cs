using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using TuPencaUy.Core.API.Model;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices;
using TuPencaUy.Core.DataServices.Services;
using TuPencaUy.Platform.DataServices.Services;

namespace TuPencaUy.Core.API.Middlewares
{
  public class RequestContentMiddleware
  {
    private ISiteService _siteService;
    private readonly RequestDelegate _next;

    public RequestContentMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceFactory _serviceFactory)
    {
      string authHeader = context.Request.Headers["Authorization"];
      bool hasTenant = context.Request.Headers.TryGetValue("currentTenant", out var currentTenant);

      if(hasTenant)
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        if(authHeader != null)
        {
          var jwtToken = tokenHandler.ReadJwtToken(authHeader.Split(' ')[1].Trim());
          var authorizedTenant = jwtToken.Claims.FirstOrDefault(x => x.Type == "currentTenant")?.Value;

          if (authorizedTenant != currentTenant)
          {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var errorResponse = new ApiResponse
            {
              Error = true,
              Message = "Unauthorized user for the site",
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            return;
          }

          try
          {
            _siteService = _serviceFactory.GetService<ISiteService>();
            string connString = _siteService.GetSiteByDomain(authorizedTenant).ConnectionString;
            _serviceFactory.CreateTenantServices(connString);
          }
          catch (Exception ex)
          {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var errorResponse = new ApiResponse
            {
              Error = true,
              Message = ex.Message,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            return;
          }
        }
      }
      else
      {
        _serviceFactory.CreatePlatformServices();
      }

      await _next(context);
    }
  }
}
