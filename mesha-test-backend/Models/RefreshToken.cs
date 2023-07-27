using System.ComponentModel.DataAnnotations;

namespace mesha_test_backend.Models;

public class RefreshToken: BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
}