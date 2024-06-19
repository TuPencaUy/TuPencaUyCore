using TuPencaUy.Core.Enums;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class AccessRequestDTO
  {
    public UserDTO? User { get; set; }
    public AccessStatusEnum? AccessStatusCode { get; set; }
    public string? AccessStatus { get; set; }

    public DateTime? RequestTime { get; set; }  
  }
}
