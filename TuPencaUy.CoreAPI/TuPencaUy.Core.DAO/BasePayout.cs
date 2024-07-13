using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuPencaUy.Core.DAO
{
  public class BasePayout : ControlDate
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [ForeignKey("Site")]
    [Column("Site_id", Order = 1)]
    public int Site_id { get; set; }

    [MaxLength(100)]
    [Column("PaypalEmail", Order = 2, TypeName = "varchar")]
    public string PaypalEmail { get; set; }

    [Column("Amount", Order = 3, TypeName = "decimal")]
    public decimal Amount { get; set; }

    [MaxLength(100)]
    [Column("TransactionID", Order = 4, TypeName = "varchar")]
    public string TransactionID { get; set; }

    [ForeignKey("Event")]
    [Column("Event_id", Order = 5)]
    public int Event_id { get; set; }
  }
}
