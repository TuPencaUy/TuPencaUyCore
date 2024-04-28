﻿namespace TuPencaUy.Platform.DAO.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using TuPencaUy.Platform.DAO.Models.Base;
  public class Role : ControlDate
  {
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    public virtual ICollection<Permission>? Permissions { get; set; }
  }
}
