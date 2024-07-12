namespace TuPencaUy.Core.API.Model.Requests
{
  public class ModifyMatchRequest
  {
    public int? FirstTeam { get; set; }
    public int? SecondTeam { get; set; }
    public int? FirstTeamScore { get; set; }
    public int? SecondTeamScore { get; set; }
    public int? Sport { get; set; }
    public DateTime? Date { get; set; }
    public bool? Finished { get; set; }
  }
}
