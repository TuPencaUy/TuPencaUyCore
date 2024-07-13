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

    [Column("Amount", Order = 2, TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    [Column("TransactionID", Order = 3, TypeName = "varchar")]
    public string TransactionID { get; set; }
  }
}
