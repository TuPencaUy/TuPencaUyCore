namespace TuPencaUy.Core.Exceptions
{
  public class EventNotFoundException : NotFoundException
  {
    public EventNotFoundException() : base(message: "Event not found") { }
    public EventNotFoundException(string message) : base(message: message) { }
  }
}
