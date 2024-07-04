using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class ModifyBetRequest
  {
    [Required]
    public required int EventId { get; set; }
    [Required]
    public required int MatchId { get; set; }
    [Required]
    public required string UserEmail { get; set; }
    public int? FirstTeamScore { get; set; }
    public int? SecondTeamScore { get; set; }
    public int? Points { get; set; }
  }
}
