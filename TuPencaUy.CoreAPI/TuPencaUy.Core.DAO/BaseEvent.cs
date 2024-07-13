using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

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
    public required DateTime? StartDate { get; set; }

    [Column("EndDate", Order = 3, TypeName = "DateTime")]
    public required DateTime? EndDate { get; set; }

    [Column("Comission", Order = 4, TypeName = "decimal(18,2)")]
    public required decimal? Comission { get; set; }

    [Column("Instantiable", Order = 5, TypeName = "bit")]
    public required bool Instantiable { get; set; } = true;

    [Column("Finished", Order = 6, TypeName = "bit")]
    public bool Finished { get; set; } = false;
    public TeamTypeEnum? TeamType { get; set; }
  }
}
