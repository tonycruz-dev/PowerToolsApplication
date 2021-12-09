using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerTools.Database;
using WinFormsUI.Repository;

namespace WinFormsUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            //Host configuration
            //docs https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-6.0
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                          .AddEnvironmentVariables();
                })
               .ConfigureServices((hostContext, services) =>
               {
                   var config = hostContext.Configuration;
                   services.AddDbContext<DataContext>(options =>
                   {
                       options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                   });
                   //Dependency injection (DI)
                   services.AddScoped<FormWindowsProducts>();
                   services.AddScoped<IProductRepository, ProductRepository>();
               });

            var host = hostBuilder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                   // string workingDirectory = Environment.CurrentDirectory;
                    //string projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.FullName + "\\Data\\";
                   // var context = services.GetRequiredService<DataContext>();
                    //context.Database.Migrate();
                    //ContextSeed.SeedAsync(context);
                    var formProducts = services.GetRequiredService<FormWindowsProducts>();
                    Application.Run(formProducts);
                    //Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured", ex.Message);
                }
            }
        }
    }
}