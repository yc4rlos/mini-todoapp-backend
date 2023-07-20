using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Models;

public class BaseEntity
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? UpdatedAt { get; set; }
}