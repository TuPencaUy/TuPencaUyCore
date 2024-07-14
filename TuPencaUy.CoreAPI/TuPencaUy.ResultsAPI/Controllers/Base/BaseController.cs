using Microsoft.AspNetCore.Mvc;
using System.Net;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.ResultsAPI.Model;

namespace TuPencaUy.ResultsAPI.Controllers.Base
{
  public class BaseController : Controller
  {
    protected GameStatusEnum CalculateGameStatus(DateTime? startTime)
    {
      if (!startTime.HasValue) return GameStatusEnum.Uknown;

      if (DateTime.Now < startTime.Value) return GameStatusEnum.Pending;
      if (DateTime.Now < startTime.Value.AddMinutes(200)) return GameStatusEnum.InProgress; // Prudential time

      return GameStatusEnum.Uknown;
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

      if (ex is BadRequestException)
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
