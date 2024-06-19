using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IAccessRequestService
  {
    List<AccessRequestDTO> GetAccessRequests(
      out int count,
      string? email,
      AccessStatusEnum? accessStatusEnum,
      int? page, int? pageSize);

    AccessRequestDTO ChangeState(AccessStatusEnum accessStatusEnum, string email);
  }
}
