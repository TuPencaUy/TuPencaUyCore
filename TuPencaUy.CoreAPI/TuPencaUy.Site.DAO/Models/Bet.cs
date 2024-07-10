using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Bet : ControlDate
  {
    [ForeignKey("Event")]
    [Column("Event_id", Order = 0)]
    public int Event_id { get; set; }

    [ForeignKey("Match")]
    [Column("Match_id", Order = 1)]
    public int Match_id { get; set; }

    [ForeignKey("User")]
    [Column("User_email", Order = 2, TypeName = "varchar")]
    public string User_email { get; set; }

    [Column("ScoreFirstTeam", Order = 3)]
    public int ScoreFirstTeam { get; set; }

    [Column("ScoreSecondTeam", Order = 4)]
    public int ScoreSecondTeam { get; set; }

    [Column("Points", Order = 5)]
    public int? Points { get; set; }

    public virtual Event Event { get; set; }
    public virtual Match Match { get; set; }
    public virtual User User { get; set; }
  }
}
