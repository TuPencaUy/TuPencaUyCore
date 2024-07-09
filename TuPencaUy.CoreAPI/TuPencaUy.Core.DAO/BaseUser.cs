﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseUser : ControlDate
  {
    [Column("Id", Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Key]
    [MaxLength(50)]
    [Column("Email", Order = 2, TypeName = "varchar")]
    public string? Email { get; set; }

    [MaxLength(100)]
    [Column("Password", Order = 3, TypeName = "varchar")]
    public string? Password { get; set; }

    [Column("paypalEmail", Order = 4)]
    public string? PaypalEmail { get; set; }
  }
}
