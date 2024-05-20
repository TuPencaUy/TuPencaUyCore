using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateSportRequest
  {
    public required string Name { get; set; }
    public required bool Tie { get; set; }
    public int? ExactPoints { get; set; }
    public int? PartialPoints { get; set; }
  }
}
