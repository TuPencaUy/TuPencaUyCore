using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.Exceptions
{
  public class EventNotFoundException : Exception
  {
    public EventNotFoundException() : base(message: "Event not found") { }
    public EventNotFoundException(string message) : base(message: message) { }
  }
}
