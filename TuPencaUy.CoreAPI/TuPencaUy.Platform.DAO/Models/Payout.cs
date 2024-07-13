using System.ComponentModel.DataAnnotations.Schema;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Payout : BasePayout
  {
    public virtual required Event Event { get; set; }
  }
}
