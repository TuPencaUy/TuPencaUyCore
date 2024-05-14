namespace TuPencaUy.Platform.DAO.Models
{
  using TuPencaUy.Core.DAO;

  public class User : BaseUser
  {
    public virtual Role? Role { get; set; }
    public virtual ICollection<Site> Sites { get; set; }
  }
}
