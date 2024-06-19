namespace TuPencaUy.Core.Exceptions
{
  public class BetNotFoundException : NotFoundException
  {
    public BetNotFoundException(string message) : base(message) { }
    public BetNotFoundException() : base(message: "Bet not foundd") { }
  }
}
