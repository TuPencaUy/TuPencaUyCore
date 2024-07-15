namespace TuPencaUy.Core.Exceptions
{
  public class UserNotAdmitedException : UnauthorizedException
  {
    public UserNotAdmitedException(string message) : base(message) { }
    public UserNotAdmitedException() : base(message: "User not admited") { }
  }
}
