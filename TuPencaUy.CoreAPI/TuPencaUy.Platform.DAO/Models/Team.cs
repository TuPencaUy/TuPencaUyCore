using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Team : BaseTeam
  {
    public required virtual Sport Sport { get; set; }
  }
}
