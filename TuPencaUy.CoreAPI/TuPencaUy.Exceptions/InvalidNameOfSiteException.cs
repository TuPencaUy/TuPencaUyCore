using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.Exceptions
{
  public class InvalidNameOfSiteException : BadRequestException
  {
    public InvalidNameOfSiteException(string message) : base(message) { }
    public InvalidNameOfSiteException() : base(message: "The site name is invalid") {}
  }
}
