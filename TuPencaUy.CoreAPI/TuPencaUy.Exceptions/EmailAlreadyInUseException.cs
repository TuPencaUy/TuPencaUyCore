namespace TuPencaUy.Core.Exceptions
{
  public class EmailAlreadyInUseException : BadRequestException
  {
    public EmailAlreadyInUseException(string message) : base(message) { }
    public EmailAlreadyInUseException() : base(message: "Email is already in use") { }
  }
}
