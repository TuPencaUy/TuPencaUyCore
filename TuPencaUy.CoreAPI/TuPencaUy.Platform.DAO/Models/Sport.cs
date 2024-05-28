using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Sport : BaseSport
  {
    public virtual ICollection<Team>? Teams { get; set; }
    public virtual ICollection<Event>? Events { get; set; }
    public virtual ICollection<Match>? Matches { get; set; }
  }
}
