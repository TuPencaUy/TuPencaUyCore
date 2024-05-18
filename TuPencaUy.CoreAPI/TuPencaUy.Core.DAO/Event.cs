﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;

namespace TuPencaUy.Core.DAO
{
  public class Event : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Column("StartDate", Order = 2, TypeName = "DateTime")]
    public required DateTime? StartDate { get; set; }

    [Column("EndDate", Order = 3, TypeName = "DateTime")]
    public required DateTime? EndDate { get; set; }

    [Column("Comission", Order = 4, TypeName = "float")]
    public required float? Comission { get; set; }
    public virtual ICollection<Sport>? Sports { get; set; }
    public virtual ICollection<Match>? Matches { get; set; }
    public TeamTypeEnum? TeamType { get; set; }
  }
}