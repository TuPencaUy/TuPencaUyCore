namespace TuPencaUy.Core.Exceptions
{
  public class SportTieException : BadRequestException
  {
    public SportTieException(string message) : base(message) { }
    public SportTieException() : base(message: "The sport doesn't admit tie") { }
  }
}
