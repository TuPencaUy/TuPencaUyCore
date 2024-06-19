namespace TuPencaUy.Core.Exceptions
{
  public class UserNotAdmitedException : UnauthorizedAccessException
  {
    public UserNotAdmitedException(string message) : base(message) { }
    public UserNotAdmitedException() : base(message: "User not admited") { }
  }
}
