﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DAO
{
  public abstract class BaseSport : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Column("Tie", Order = 2, TypeName = "bit")]
    public required bool Tie{ get; set; }

    [Column("ExactPoints", Order = 3, TypeName = "int")]
    public int? ExactPoints { get; set; }

    [Column("PartialPoints", Order = 4, TypeName = "int")]
    public int? PartialPoints { get; set; }
  }
}
