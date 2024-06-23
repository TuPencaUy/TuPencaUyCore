using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TuPencaUy.Core.DTOs;

namespace TuPencaUy.DTOs
{
  public class UserDTO
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }
    public string? Email { get; set; }
    public RoleDTO? Role { get; set; }
    public SiteDTO? Site { get; set; }
    public List<EventDTO>? Events { get; set; }
  }
}
