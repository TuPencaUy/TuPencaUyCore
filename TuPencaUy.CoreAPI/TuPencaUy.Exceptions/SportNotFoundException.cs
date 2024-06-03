namespace TuPencaUy.Core.Exceptions
{
  public class SportNotFoundException : NotFoundException
  {
    public SportNotFoundException(string message) : base(message) { }
    public SportNotFoundException() : base(message: "Sport not found") { }
  }
}
