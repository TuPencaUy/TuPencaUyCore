using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateEventRequest
  {
    [Required]
    public required string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public float? Comission { get; set; }
    public TeamTypeEnum TeamType { get; set; }
  }
}
