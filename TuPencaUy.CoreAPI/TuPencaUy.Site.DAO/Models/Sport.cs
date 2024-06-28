using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Sport : BaseSport
  {
    public int RefSport { get; set; }
    public virtual ICollection<Event>? Events { get; set; }
  }
}
