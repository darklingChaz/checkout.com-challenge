
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Installers;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => {

                    new CoreInstaller().ConfigureServices(services, context);
                    new ServicesInstaller().ConfigureServices(services, context);

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
