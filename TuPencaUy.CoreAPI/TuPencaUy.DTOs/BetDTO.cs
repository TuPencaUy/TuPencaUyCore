using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class BetDTO
  {
    public MatchDTO Match { get; set; }
    public EventDTO Event { get; set; }
    public UserDTO User { get; set; }
    public int? ScoreFirstTeam { get; set; }
    public int? ScoreSecondTeam { get; set; }
    public int? Points { get; set; }
  }
}
