using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.DAO
{
  public class Match : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [ForeignKey("Team")]
    [Column("FirstTeam", Order = 1)]
    public int? FirstTeam { get; set; }

    [ForeignKey("Team")]
    [Column("SecondTeam", Order = 2)]
    public int? SecondTeam { get; set; }

    [Column("FirstTeamScore", Order = 3)]
    public int? FirstTeamScore { get; set; }

    [Column("SecondTeamScore", Order = 4)]
    public int? SecondTeamScore { get; set; }

    [Column("Date", Order = 5)]
    public DateTime? Date { get; set; }

    [ForeignKey("Sport")]
    [Column("Sport", Order = 6)]
    public int? Sport { get; set; }
  }
}
