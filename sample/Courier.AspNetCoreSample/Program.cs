using Courier.AspNetCoreSample.Listeners;
using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Courier.AspNetCoreSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            {
                using var scope = host.Services.CreateScope(); 
            
                var courier = scope.ServiceProvider.GetRequiredService<ICourier>();
                courier.Subscribe<SomethingHappenedEvent, SomethingHappenedEventListener>(
                    scope.ServiceProvider.GetRequiredService<SomethingHappenedEventListener>());
            }
            
            

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder => builder.AddConsole())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
