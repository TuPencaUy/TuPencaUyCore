using TuPencaUy.Core.Exceptions;

namespace TuPencaUy.Exceptions
{
  public class UserNotFoundException : NotFoundException
  {
    public UserNotFoundException() : base(message: "User not found") { }
    public UserNotFoundException(string message) : base(message: message) { }
  }
}
