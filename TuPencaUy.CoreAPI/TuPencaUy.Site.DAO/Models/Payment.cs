using System.ComponentModel.DataAnnotations.Schema;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Payment : BasePayment
  {
    [ForeignKey("User")]
    [Column("User_email", Order = 4, TypeName = "varchar")]
    public string User_email { get; set; }
    public virtual required Event Event { get; set; }
    public virtual required User User { get; set; }
  }
}
