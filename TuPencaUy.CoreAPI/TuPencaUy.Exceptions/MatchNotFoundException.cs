using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.Exceptions
{
  public class MatchNotFoundException : NotFoundException
  {
    public MatchNotFoundException(string meessage) : base(meessage) { }
    public MatchNotFoundException() : base(message: "Match not found") { }
  }
}
