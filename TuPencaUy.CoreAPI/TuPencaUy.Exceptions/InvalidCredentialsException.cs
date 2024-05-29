using TuPencaUy.Core.Exceptions;

namespace TuPencaUy.Exceptions
{
  public class InvalidCredentialsException : BadRequestException
  {
    public InvalidCredentialsException() : base(message: "Invalid credentials") { }
    public InvalidCredentialsException(string message) : base(message: message) { }
  }
}
