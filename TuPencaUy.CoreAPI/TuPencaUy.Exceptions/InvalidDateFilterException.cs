namespace TuPencaUy.Core.Exceptions
{
  public class InvalidDateFilterException : BadRequestException
  {
    public InvalidDateFilterException(string message) : base(message) { }
    public InvalidDateFilterException() : base(message: "The until date must be greater than from time") { }
  }
}
