using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseTeam : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Column("Logo", Order = 2, TypeName = "image")]
    public byte[]? Logo { get; set; }

    [ForeignKey("Sport")]
    [Column("Sport_id", Order = 3)]
    public int? Sport_id { get; set; }

    public TeamTypeEnum? TeamType { get; set; }
  }
}
