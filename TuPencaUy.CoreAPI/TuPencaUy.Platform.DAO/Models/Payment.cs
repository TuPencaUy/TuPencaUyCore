using System.ComponentModel.DataAnnotations.Schema;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Payment : BasePayment
  {
    [Column("User_email", Order = 4, TypeName = "varchar")]
    public string User_email { get; set; }

    [ForeignKey("Site")]
    [Column("Site_id", Order = 5, TypeName = "int")]
    public int? Site_id { get; set; }

    public virtual required Event Event { get; set; }
    public virtual Site? Site { get; set; }
  }
}
