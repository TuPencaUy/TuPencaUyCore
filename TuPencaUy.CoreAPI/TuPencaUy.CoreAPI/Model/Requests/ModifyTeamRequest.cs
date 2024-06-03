using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class ModifyTeamRequest
  {
    public string? Name { get; set; }
    public byte[]? Logo { get; set; }
    public TeamTypeEnum? TeamType { get; set; }
    public int? Sport { get; set; }
  }
}
