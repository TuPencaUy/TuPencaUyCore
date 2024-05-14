namespace TuPencaUy.Platform.DAO.Models
{
  using TuPencaUy.Core.DAO;

  public class Permission : BasePermission
  {
    public virtual ICollection<Role>? Roles { get; set; }
  }
}
