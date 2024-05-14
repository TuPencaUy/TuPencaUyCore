namespace TuPencaUy.Site.DAO.Models
{
  using System.ComponentModel.DataAnnotations.Schema;
  using System.ComponentModel.DataAnnotations;
  using TuPencaUy.Core.DAO;

  public class Permission : BasePermission
  {
    public virtual ICollection<Role>? Roles { get; set; }
  }
}
