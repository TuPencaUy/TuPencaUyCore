using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateMatchRequest
  {
    public int EventId { get; set; }
    public int? FirstTeam { get; set; }
    public int? SecondTeam { get; set; }
    public int? FirstTeamScore { get; set; }
    public int? SecondTeamScore { get; set; }
    public DateTime? Date { get; set; }
  }
}
