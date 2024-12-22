using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Diagnostics;

namespace MD1
{
    public class SchoolSystemContextFactory : IDesignTimeDbContextFactory<SchoolSystemContext>
    {
        //Izveido DBContext instanci, izmantojot konfigurāciju (1)
        public SchoolSystemContext CreateDbContext(string[] args)
        {
            try
            {
                var projectDirectory = @"C:\Temp\MD2";

                var appSettingsPath = Path.Combine(projectDirectory, "Appsettings.json");

                if (!File.Exists(appSettingsPath))
                {
                    throw new FileNotFoundException($"appsettings.json not found at {appSettingsPath}");
                }

                Console.WriteLine($"appsettings.json found at {appSettingsPath}");

                var config = new ConfigurationBuilder()
                    .SetBasePath(projectDirectory)
                    .AddJsonFile("Appsettings.json", optional: false, reloadOnChange: true) 
                    .Build();

                var connectionString = config.GetConnectionString("SchoolDatabase");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'SchoolDatabase' is missing or empty in appsettings.json.");
                }

                Console.WriteLine($"Connection string found: {connectionString}");

                var optionsBuilder = new DbContextOptionsBuilder<SchoolSystemContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using (var dbContext = new SchoolSystemContext(optionsBuilder.Options))
                {
                    try
                    {
                        dbContext.Database.OpenConnection();
                        Debug.WriteLine("Connection successful!"); 
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Connection failed: {ex.Message}");
                    }
                }

                Console.WriteLine("DbContext options are configured correctly.");
                return new SchoolSystemContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateDbContext: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/