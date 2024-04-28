namespace TuPencaUy.Platform.DAO.Models
{
  using System.ComponentModel.DataAnnotations.Schema;
  using System.ComponentModel.DataAnnotations;
  using TuPencaUy.Platform.DAO.Models.Base;
  public class Permission : ControlDate
  {
    [Key]
    [Column("Id", Order = 0)]
    public int Id { get; set; }

    [Column("Name", Order = 1, TypeName = "varchar")]
    public required string Name { get; set; }

    [Column("Description", Order = 2, TypeName = "varchar")]
    public required string Description { get; set; }

    public virtual ICollection<Role>? Roles { get; set;}
  }
}
