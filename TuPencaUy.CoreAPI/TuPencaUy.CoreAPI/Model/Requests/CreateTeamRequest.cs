using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class CreateTeamRequest
  {
    public required string Name { get; set; }
    public byte[]? Logo { get; set; }
  }
}
