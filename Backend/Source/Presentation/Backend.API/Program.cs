using Backend.Domain;
using Backend.Library.Logging.Interfaces;
using Backend.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace Backend.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggingService = services.GetService<ILoggingService>();
                loggingService.DisableDbLog();

                try
                {
                    //// comment this if you don't want seed values in migrations
                    var contextInterface = services.GetService<IApplicationDbContext>();
                    var context = (ApplicationDbContext)contextInterface;
                    context.Database.Migrate();
                    loggingService.LogInfo("Database successfully migrated and seeded").Wait();

                    loggingService.LogInfo("Application started.").Wait();
                    host.Run();
                }
                catch (Exception ex)
                {
                    loggingService.LogError(ex, "Internal server error in Main. Perhaps DB failed to migrate or seed.").Wait();
                    throw new Exception("Check your internet connection. DB Migration possibly failed also.", ex);
                }
                finally
                {
                    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                    NLog.LogManager.Shutdown();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace); // App settings override this
                })
                .UseNLog();
    }
}
