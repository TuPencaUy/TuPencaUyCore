using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateMatchRequest
  {
    [Required]
    public int EventId { get; set; }
    public int? FirstTeam { get; set; }
    public int? SecondTeam { get; set; }
    public int? FirstTeamScore { get; set; }
    public int? SecondTeamScore { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int Sport { get; set; } 
  }
}
