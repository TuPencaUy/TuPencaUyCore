namespace TuPencaUy.Platform.DAO.Models
{
  using TuPencaUy.Core.DAO;
  
  public class Event : BaseEvent
  {
    public virtual ICollection<Sport>? Sports { get; set; }
    public virtual ICollection<Match>? Matches { get; set; }
  }
}
