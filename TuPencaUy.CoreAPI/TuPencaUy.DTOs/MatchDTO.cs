namespace TuPencaUy.Core.DTOs
{
  public class MatchDTO
  {
    public int? Id { get; set; }
    public TeamDTO? FirstTeam { get; set; }
    public TeamDTO? SecondTeam { get; set; }
    public int? FirstTeamScore { get; set; }
    public int? SecondTeamScore { get; set; }
    public SportDTO Sport { get; set; }
    public DateTime? Date { get; set; }
  }
}
