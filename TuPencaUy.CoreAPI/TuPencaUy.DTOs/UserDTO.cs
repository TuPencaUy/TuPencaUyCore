using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.DTOs
{
  public class UserDTO
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public RoleDTO? Role { get; set; }
  }
}
