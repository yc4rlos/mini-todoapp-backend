using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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

    public ICollection<Task> Tasks { get; set; }
    protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade);
    }

}