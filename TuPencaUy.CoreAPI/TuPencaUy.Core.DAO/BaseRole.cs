using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseRole : ControlDate
  {
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

  }
}
