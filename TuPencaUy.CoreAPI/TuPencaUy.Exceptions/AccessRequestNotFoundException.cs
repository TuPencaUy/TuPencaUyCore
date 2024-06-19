namespace TuPencaUy.Core.Exceptions
{
  public class AccessRequestNotFoundException : NotFoundException
  {
    public AccessRequestNotFoundException(string message) : base(message) { }
    public AccessRequestNotFoundException() : base(message: "Access request not found") { }
  }
}
