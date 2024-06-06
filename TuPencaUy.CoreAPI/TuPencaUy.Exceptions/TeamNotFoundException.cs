
namespace TuPencaUy.Core.Exceptions
{
  public class TeamNotFoundException : NotFoundException
  {
    public TeamNotFoundException(string message) : base(message) { }
    public TeamNotFoundException() : base(message: "Team not found") { }
  }
}
