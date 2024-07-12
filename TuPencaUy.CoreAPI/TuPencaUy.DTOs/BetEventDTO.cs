using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DTOs
{
  public class BetEventDTO
  {
    public string EventName { get; set; }
    public int UsersCount { get; set; }
    public decimal Prize { get; set; }
    public bool Finished { get; set; }
    public decimal AmountCollected { get; set; }  
    public int TotalBets { get; set; }
    public int TotalHits { get; set; }
    public int TotalPartialHits { get; set; }
    public int TotalErrors {
      get
      {
        if (TotalBets == 0) return 0;
        return TotalBets - TotalHits - TotalPartialHits; 
      }
    }
    public decimal HitsPercentage
    {
      get
      {
        if (TotalBets == 0 || TotalHits == 0) return 0;
        return (decimal)TotalHits / (decimal)TotalBets;
      }
    }
    public decimal PartialHitsPercentage
    {
      get
      {
        if (TotalBets == 0 || TotalPartialHits == 0) return 0;
        return (decimal)TotalPartialHits / (decimal)TotalBets;
      }
    }
  }
}
