using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseMatch : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [ForeignKey("Team")]
    [Column("FirstTeam", Order = 2)]
    public int? FirstTeam { get; set; }

    [ForeignKey("Team")]
    [Column("SecondTeam", Order = 3)]
    public int? SecondTeam { get; set; }

    [Column("FirstTeamScore", Order = 4)]
    public int? FirstTeamScore { get; set; }

    [Column("SecondTeamScore", Order = 5)]
    public int? SecondTeamScore { get; set; }
  }
}
