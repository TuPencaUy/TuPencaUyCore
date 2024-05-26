namespace TuPencaUy.Site.DAO.Models
{
  using TuPencaUy.Core.DAO;

  public class User : BaseUser
  {
    public virtual ICollection<BaseEvent>? Events { get; set; }
    public virtual Role? Role { get; set; }
  }
}
