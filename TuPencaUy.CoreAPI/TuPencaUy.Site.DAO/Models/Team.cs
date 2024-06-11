using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Team : BaseTeam
  {
    public required virtual Sport Sport { get; set; }
    public int RefTeam { get; set; }
  }
}
