namespace TuPencaUy.Exceptions
{
  public class UserNotFoundException : Exception
  {
    public UserNotFoundException() : base(message: "User not found") { }
    public UserNotFoundException(string message) : base(message: message) { }
  }
}
