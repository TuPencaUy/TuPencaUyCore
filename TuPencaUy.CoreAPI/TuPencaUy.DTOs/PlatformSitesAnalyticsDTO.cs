namespace TuPencaUy.Core.DTOs
{
  public class PlatformSitesAnalyticsDTO
  {
    public int SitesCount {
      get
      {
        return Sites.Count;
      }
    }
    public int TotalUsers { get
      {
        return Sites.Sum(x => x.Item2);
      }
    }
  
    public List<Tuple<int, int>> Sites { get; set; }
  }
}
