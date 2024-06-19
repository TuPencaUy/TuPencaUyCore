namespace TuPencaUy.Core.Exceptions
{
  public class MatchAlreadyStartedException : BadRequestException
  {
    public MatchAlreadyStartedException(string message) : base(message) { }
    public MatchAlreadyStartedException() : base(message: "The match has already started") { }

  }
}
