namespace TuPencaUy.Core.DTOs
{
  public class BetUserDTO
  {
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Points { get; set; }
    public int PredictedMatches { get; set; }
    public int Hits { get; set; }
    public int PartialHits { get; set; }
    public int Errors
    {
      get
      {
        return PredictedMatches - Hits - PartialHits;
      }
    }
    public decimal HitsPercentage {
      get
      {
        if (PredictedMatches == 0 || Hits == 0) return 0;

        return (decimal)Hits / (decimal)PredictedMatches * 100;
      }
    }
    public decimal PartialHitsPercentage {
      get
      {
        if (PredictedMatches == 0 || PartialHits == 0) return 0;
        return (decimal)PartialHits / (decimal)PredictedMatches * 100;
      }
    }
  }
}
