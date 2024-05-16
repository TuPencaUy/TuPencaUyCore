using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.DTOs
{
  public class SportDTO
  {
    public required string Name { get; set; }
    public required bool Tie { get; set; }
    public int? ExactPoints { get; set; }
    public int? PartialPoints { get; set; }
  }
}
