﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseTeam : ControlDate
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [MaxLength(50)]
    [Column("Logo", Order = 2)]
    public byte[]? Logo { get; set; }
  }
}
