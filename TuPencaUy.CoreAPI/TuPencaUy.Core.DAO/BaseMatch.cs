using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseMatch : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [ForeignKey("FirstTeam")]
    [Column("FirstTeam", Order = 1)]
    public int? FirstTeam_id { get; set; }

    [ForeignKey("SecondTeam")]
    [Column("SecondTeam", Order = 2)]
    public int? SecondTeam_id { get; set; }

    [Column("FirstTeamScore", Order = 3)]
    public int? FirstTeamScore { get; set; }

    [Column("SecondTeamScore", Order = 4)]
    public int? SecondTeamScore { get; set; }

    [Column("Date", Order = 5)]
    public DateTime? Date { get; set; }

    [ForeignKey("Sport")]
    [Column("Sport_id", Order = 6)]
    public int? Sport_id { get; set; }

    [ForeignKey("Event")]
    [Column("Event_id", Order = 7)]
    public int Event_id { get; set; }
  }
}
