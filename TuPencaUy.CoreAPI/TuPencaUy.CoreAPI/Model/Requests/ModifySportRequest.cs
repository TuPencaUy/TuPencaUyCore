using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class ModifySportRequest
  {
    public string? Name { get; set; }
    public bool? Tie { get; set; }
    public int? ExactPoints { get; set; }
    public int? PartialPoints { get; set; }
  }
}
