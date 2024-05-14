namespace TuPencaUy.Site.DAO.Models
{
  using TuPencaUy.Core.DAO;

  public class Role : BaseRole
  {
    public virtual ICollection<Permission>? Permissions { get; set; }
  }
}
