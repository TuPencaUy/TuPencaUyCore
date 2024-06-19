using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TuPencaUy.Core.DAO;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Site.DAO.Models
{
  public class AccessRequest : ControlDate
  {
    [Key]
    [ForeignKey("User")]
    [Column("User_email", Order = 0)]
    public string User_email { get; set; }
    [Column("AccessStatus", Order = 1)]
    public AccessStatusEnum AccessStatus { get; set; }

    [Column("RequestTime", Order = 2)]
    public DateTime RequestTime { get; set; }
    public virtual User User { get; set; }
  }
}
