namespace TuPencaUy.Core.Exceptions
{
  public abstract class UnauthorizedException : Exception
  {
    public UnauthorizedException(string message) : base(message) { }
  }
}
