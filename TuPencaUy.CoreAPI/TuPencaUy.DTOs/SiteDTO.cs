using System.Text.Json.Serialization;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DTOs
{
  public class SiteDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public  string Domain { get; set; }

    [JsonIgnore]
    public string ConnectionString { get; set; }
    public SiteAccessTypeEnum? AccessType { get; set; }
    public SiteColorEnum? Color { get; set; }
  }
}
