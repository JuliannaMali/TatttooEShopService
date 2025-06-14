using System.ComponentModel.DataAnnotations;

namespace UserDomain.Models.Entities;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    public int UserId { get; set; }
}
