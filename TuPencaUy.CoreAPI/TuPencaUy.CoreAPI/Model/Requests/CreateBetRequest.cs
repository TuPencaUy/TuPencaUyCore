using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateBetRequest
  {
    [Required]
    public required int EventId { get; set; }
    [Required]
    public required int MatchId { get; set; }
    [Required]
    public required string UserEmail { get; set; }
    [Required]
    public required int FirstTeamScore { get; set; }
    [Required]
    public required int SecondTeamScore { get; set; }
  }
}
