using mesha_test_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace mesha_test_backend.Data;

public class TasksDatabaseContext: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "Tasks");
    }
    
    public DbSet<User> Users { get; set; }
}