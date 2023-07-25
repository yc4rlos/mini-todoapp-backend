using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Models;

public class Task: BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }
 
    public string Description { get; set; }

    public bool Complete { get; set; } = false;
    
    [Required]
    public Guid UserId { get; set; }
    
    
}