using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.API.Model.Requests
{
  public class SiteRequest
  {
    public int? Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Domain { get; set; }
    public SiteAccessTypeEnum? AccessType { get; set; }
    public SiteColorEnum? Color { get; set; }
    public string? PaypalEmail { get; set; }
  }
}
