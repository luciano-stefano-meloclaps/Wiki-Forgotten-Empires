using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>()))
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationContext>>();

                try
                {
                    // Ensure the database is created
                    context.Database.EnsureCreated();

                    // Check if the database has already been seeded
                    if (context.Ages.Any())
                    {
                        logger.LogInformation("Database already contains data. Skipping seeding.");
                        return;
                    }

                    logger.LogInformation("Database is empty. Seeding from backup_datos.sql...");

                    // Refined path resolution for Architect Audit
                    // 1. Check relative to current working directory (Local Dev)
                    string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "database", "backup_datos.sql");
                    
                    if (!File.Exists(scriptPath))
                    {
                        // 2. Check relative to AppContext.BaseDirectory (Published/Unit Tests)
                        scriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "database", "backup_datos.sql");
                    }
                    
                    if (!File.Exists(scriptPath))
                    {
                        // 3. Fallback for Docker environment mapping
                        scriptPath = "/app/database/backup_datos.sql";
                    }

                    if (File.Exists(scriptPath))
                    {
                        string sql = File.ReadAllText(scriptPath);
                        context.Database.ExecuteSqlRaw(sql);
                        logger.LogInformation("Database seeded successfully.");
                    }
                    else
                    {
                        logger.LogWarning("Seed script not found at: {Path}", scriptPath);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
