namespace TuPencaUy.Exceptions
{
  public class InvalidCredentialsException : Exception
  {
    public InvalidCredentialsException() : base(message: "Invalid credentials") { }
    public InvalidCredentialsException(string message) : base(message: message) { }
  }
}
