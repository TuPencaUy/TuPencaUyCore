namespace TuPencaUy.Core.Exceptions
{
  public class MatchDoesntStartException : BadRequestException
  {
    public MatchDoesntStartException(string message) : base(message) { }
    public MatchDoesntStartException() : base(message: "The match doesn't start") { }
  }
}
