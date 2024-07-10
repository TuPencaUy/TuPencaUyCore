using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.DAO
{
  public class BasePayment : ControlDate
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [ForeignKey("Event")]
    [Column("Event_id", Order = 1)]
    public int Event_id { get; set; }

    [ForeignKey("User")]
    [Column("User_email", Order = 2, TypeName = "varchar")]
    public string User_email { get; set; }

    [Column("Amount", Order = 3, TypeName = "decimal")]
    public decimal Amount { get; set; }

    [Column("TransactionID", Order = 4, TypeName = "varchar")]
    public string TransactionID { get; set; }

    [Column("Date", Order = 5, TypeName = "Date")]
    public DateTime Date { get; set; }
  }
}
