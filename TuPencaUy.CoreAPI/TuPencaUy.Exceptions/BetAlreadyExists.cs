namespace TuPencaUy.Core.Exceptions
{
  public class BetAlreadyExists : BadRequestException
  {
    public BetAlreadyExists(string message) : base(message) { }
    public BetAlreadyExists() : base(message: "Bet already exists") { }
  }
}
