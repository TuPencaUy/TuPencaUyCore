using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Site.DAO.Models
{
  public class Payment
  {
    [ForeignKey("Event")]
    [Column("Event_id", Order = 0)]
    public int Event_id { get; set; }

    [ForeignKey("User")]
    [Column("User_email", Order = 1, TypeName = "varchar")]
    public string User_email { get; set; }

    [Column("Amount", Order = 2, TypeName = "decimal")]
    public decimal Amount { get; set; }

    [Column("TransactionID", Order = 2, TypeName = "varchar")]
    public string TransactionID { get; set; }

    public virtual Event Event { get; set; }
    public virtual User User { get; set; }
  }
}
