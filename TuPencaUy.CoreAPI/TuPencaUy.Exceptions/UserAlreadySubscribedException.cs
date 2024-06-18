namespace TuPencaUy.Core.Exceptions
{
  public class UserAlreadySubscribedException : BadRequestException
  {
    public UserAlreadySubscribedException() : base(message: "User already subscribed") { }
    public UserAlreadySubscribedException(string message) : base(message) { }
  }
}
