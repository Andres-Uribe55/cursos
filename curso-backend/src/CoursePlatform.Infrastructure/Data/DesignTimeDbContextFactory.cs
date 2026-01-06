using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CoursePlatform.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Try to find appsettings.json
        // Assuming we are running from solution root or project folder
        
        var basePath = Directory.GetCurrentDirectory();
        // Adjust path if we are in the project folder
        if (basePath.EndsWith("CoursePlatform.Infrastructure"))
        {
             basePath = Path.Combine(basePath, "..", "CoursePlatform.API");
        }
        else if (basePath.EndsWith("curso-backend"))
        {
             basePath = Path.Combine(basePath, "src", "CoursePlatform.API");
        }
        
        // Fallback or explicit check
        if (!File.Exists(Path.Combine(basePath, "appsettings.json"))) 
        {
             // Try absolute path if known or just fail gracefully?
             // Lets assume standard structure
        }

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}
