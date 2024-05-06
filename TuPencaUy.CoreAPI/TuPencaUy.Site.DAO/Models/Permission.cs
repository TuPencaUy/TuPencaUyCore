namespace TuPencaUy.Site.DAO.Models
{
  using System.ComponentModel.DataAnnotations.Schema;
  using System.ComponentModel.DataAnnotations;
  using TuPencaUy.Site.DAO.Models.Base;

  public class Permission : ControlDate
  {
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [MaxLength(100)]
    [Column("Description", Order = 2, TypeName = "varchar")]
    public required string Description { get; set; }

    public virtual ICollection<Role>? Roles { get; set; }
  }
}
