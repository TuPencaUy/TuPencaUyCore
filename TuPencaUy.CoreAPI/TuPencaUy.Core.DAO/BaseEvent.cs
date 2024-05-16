using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseEvent : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Column("StartDate", Order = 2, TypeName = "DateTime")]
    public required DateTime StartDate { get; set; }

    [Column("EndDate", Order = 3, TypeName = "DateTime")]
    public required DateTime EndDate { get; set; }

    [Column("Comission", Order = 4, TypeName = "float")]
    public required float Comission { get; set; }
  }
}
