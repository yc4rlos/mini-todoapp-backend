using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Models;

public class User: BaseEntity
{
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    public string? Lastname { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(25)]
    [MinLength(8)]
    public string Password { get; set; }

    public ICollection<Models.Task> Tasks { get; set; }

}