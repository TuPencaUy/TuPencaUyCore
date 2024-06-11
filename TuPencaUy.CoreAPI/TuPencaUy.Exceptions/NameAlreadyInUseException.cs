namespace TuPencaUy.Core.Exceptions
{
  public class NameAlreadyInUseException : BadRequestException
  {
    public NameAlreadyInUseException(string message) : base(message) { }
    public NameAlreadyInUseException() : base(message: "The name is already in use") { }
  }
}
