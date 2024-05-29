using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.Exceptions
{
  public class SiteNotFoundException : NotFoundException
  {
    public SiteNotFoundException() : base(message: "Site not found") { }
    public SiteNotFoundException(string message) : base(message: message) { }
  }
}
