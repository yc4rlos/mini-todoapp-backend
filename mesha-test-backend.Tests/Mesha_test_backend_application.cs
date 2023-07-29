using mesha_test_backend.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace mesha_test_backend.Tests;

public class Mesha_test_backend_application: WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TasksDatabaseContext>));
            services.AddDbContext<TasksDatabaseContext>(options =>
                options.UseInMemoryDatabase("TasksDatabase", root));
        });

        return base.CreateHost(builder);
    }
}