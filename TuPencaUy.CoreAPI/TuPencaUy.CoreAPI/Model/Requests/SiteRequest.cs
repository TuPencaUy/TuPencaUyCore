using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class SiteRequest
  {
    public string Name { get; set; }
    public string Domain { get; set; }
    public SiteAccessTypeEnum? AccessType { get; set; }
    public SiteColorEnum? Color { get; set; }
  }
}
