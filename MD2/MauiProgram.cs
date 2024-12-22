using MD1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MD2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var basePath = Path.Combine(AppContext.BaseDirectory);

            if (!File.Exists(Path.Combine(basePath, "Appsettings.json")))
            {
                throw new FileNotFoundException($"Appsettings.json not found at {Path.Combine(basePath, "Appsettings.json")}");
            }

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Appsettings.json", optional: false, reloadOnChange: true);

            builder.Configuration.AddConfiguration(configBuilder.Build());

            Console.WriteLine($"Configuration Base Path: {basePath}");
            Console.WriteLine($"Appsettings.json Exists: {File.Exists(Path.Combine(basePath, "Appsettings.json"))}");

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<SchoolSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDatabase"))
            );

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

/**
 Visu kodu būvēju gan ar lekciju ierakstu palīdzību, gan ar ChatGPT
 **/