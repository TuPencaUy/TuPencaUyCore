using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateTeamRequest
  {
    [Required]
    public required string Name { get; set; }
    public byte[]? Logo { get; set; }
    public TeamTypeEnum TeamType { get; set; }
    public int Sport { get; set; }
  }
}
