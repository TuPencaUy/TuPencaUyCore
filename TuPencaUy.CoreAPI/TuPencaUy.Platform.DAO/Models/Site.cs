using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.DAO;

namespace TuPencaUy.Platform.DAO.Models
{
  public class Site : ControlDate
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [MaxLength(50)]
    [Column("Domain", Order = 2, TypeName = "varchar")]
    public required string Domain { get; set; }

    [MaxLength(500)]
    [Column("ConnectionString", Order = 3, TypeName = "varchar")]
    public required string ConnectionString { get; set; }
    public SiteAccessTypeEnum? AccessType { get; set; }
    public SiteColorEnum? Color { get; set; }
    public string? PaypalEmail { get; set; }
  }
}
