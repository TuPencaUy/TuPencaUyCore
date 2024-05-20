using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateSportRequest
  {
    [Required]
    public required string Name { get; set; }
    [Required]
    public required bool Tie { get; set; }
    public int? ExactPoints { get; set; }
    public int? PartialPoints { get; set; }
  }
}
