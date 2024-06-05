using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Event : BaseEvent
  {
    public virtual ICollection<Sport>? Sports { get; set; }
    public virtual ICollection<Match>? Matches { get; set; }
  }
}
