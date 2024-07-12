using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Payment : BasePayment
  {
    public virtual required Event Event { get; set; }
    public virtual required User User { get; set; }
  }
}
