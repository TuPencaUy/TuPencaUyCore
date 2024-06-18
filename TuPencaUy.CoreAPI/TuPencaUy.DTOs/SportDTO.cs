using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.DTOs
{
  public class SportDTO
  {
    public int? Id{ get; set; }
    public required string Name { get; set; }
    public required bool Tie { get; set; }
    public int? ExactPoints { get; set; }
    public int? PartialPoints { get; set; }

    public int? ReferenceSport { get; set; }
  }
}
