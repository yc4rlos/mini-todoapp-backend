using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace mesha_test_backend.Models;

public class Task: BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }
 
    [Required]
    public string Description { get; set; }

    public bool Complete { get; set; } = false;
    
    [Required]
    public string UserId { get; set; }
    
    public User User { get; set; }
    
    protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>()
            .HasOne( t => t.User)
            .WithMany(u => u.Tasks).IsRequired();
    }
}