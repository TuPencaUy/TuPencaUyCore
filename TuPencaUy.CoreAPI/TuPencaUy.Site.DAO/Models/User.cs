namespace TuPencaUy.Site.DAO.Models
{
  using System.ComponentModel.DataAnnotations.Schema;
  using TuPencaUy.Core.DAO;

  public class User : BaseUser
  {
    [ForeignKey("Role")]
    [Column("RoleId", Order = 4)]
    public int? roleId { get; set; }

    [Column("paypalEmail", Order = 5)]
    public string? PaypalEmail { get; set; }
    public virtual ICollection<Event>? Events { get; set; }
    public virtual ICollection<Bet>? Bets { get; set; }
    public virtual Role? Role { get; set; }
    public virtual AccessRequest AccessRequest { get; set; }
  }
}
