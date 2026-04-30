using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var notionSecret = configuration?["Notion:Secret"];
            var notionDatabaseId = configuration?["Notion:DatabaseId"];

            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>() ))
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ApplicationContext>>();

                try
                {
                    // Ensure the database is created, even if we only use Notion-synced data.
                    context.Database.EnsureCreated();

                    if (!string.IsNullOrWhiteSpace(notionSecret) && !string.IsNullOrWhiteSpace(notionDatabaseId))
                    {
                        logger.LogInformation("Notion is configured. Skipping local database seeding.");
                        return;
                    }

                    // Check if the database has already been seeded
                    if (context.Ages.Any())
                    {
                        logger.LogInformation("Database already contains data. Skipping seeding.");
                        return;
                    }

                    logger.LogInformation("Database is empty. Seeding from backup_datos.sql...");

                    string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "database", "backup_datos.sql");

                    if (!File.Exists(scriptPath))
                    {
                        scriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "database", "backup_datos.sql");
                    }

                    if (!File.Exists(scriptPath))
                    {
                        scriptPath = "/app/database/backup_datos.sql";
                    }

                    if (File.Exists(scriptPath))
                    {
                        if (context.Database.IsSqlite())
                        {
                            string sql = File.ReadAllText(scriptPath);
                            context.Database.ExecuteSqlRaw(sql);
                            logger.LogInformation("Database seeded successfully from SQLite script.");
                        }
                        else
                        {
                            logger.LogInformation("Skipping SQLite seed script on non-SQLite database. Tables were created by EF Core.");
                        }
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
