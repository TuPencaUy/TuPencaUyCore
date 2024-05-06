namespace TuPencaUy.Site.DAO.Models
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using TuPencaUy.Site.DAO.Models.Base;

  public class User : ControlDate
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

    [ForeignKey("Role")]
    [Column("RoleId", Order = 4)]
    public int? roleId { get; set; }
    public virtual Role? Role { get; set; }
  }
}
