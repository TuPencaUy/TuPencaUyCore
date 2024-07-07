namespace TuPencaUy.Core.DTOs
{
  public class BetMatchDTO
  {
    public string EventName { get; set; }
    public DateTime MatchDate { get; set; }
    public string FirstTeam { get; set; }
    public string SecondTeam { get; set; }
    public int TotalBets { get; set; }
    public int FirstTeamWinnerBets { get; set; }
    public int SecondTeamWinnerBets { get; set; }
    public int TieBets {  get; set; }
    public List<BetScoreDTO>? PopularBets { get; set; }
    public decimal FirstTeamWinnerPercentage {
      get
      {
        if(TotalBets == 0 || FirstTeamWinnerBets == 0) return 0;
        return (decimal)FirstTeamWinnerBets / (decimal)TotalBets;
      }
    }
    public decimal SecondTeamWinnerPercentage
    {
      get
      {
        if (TotalBets == 0 || SecondTeamWinnerBets == 0) return 0;
        return (decimal)SecondTeamWinnerBets / (decimal)TotalBets;
      }
    }
    public decimal TiePercentage
    {
      get
      {
        if (TotalBets == 0 || TieBets == 0) return 0;
        return (decimal)TieBets / (decimal)TotalBets;
      }
    }
  }
}
