namespace TuPencaUy.Platform.DAO.Models
{
  using System.ComponentModel.DataAnnotations.Schema;
  using TuPencaUy.Core.DAO;

  public class User : BaseUser
  {

    [ForeignKey("Role")]
    [Column("RoleId", Order = 4)]
    public int? roleId { get; set; }
    public virtual Role? Role { get; set; }
    public virtual ICollection<Site> Sites { get; set; }
  }
}
