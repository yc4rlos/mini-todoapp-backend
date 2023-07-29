using mesha_test_backend.Models;
using Microsoft.EntityFrameworkCore;
using Task = mesha_test_backend.Models.Task;

namespace mesha_test_backend.Data;

public class TasksDatabaseContext: DbContext
{
    public TasksDatabaseContext(DbContextOptions<TasksDatabaseContext> options): base(options) {}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "Tasks");
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}