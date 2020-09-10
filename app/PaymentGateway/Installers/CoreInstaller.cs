



using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Services;

namespace PaymentGateway.Installers {


    public class CoreInstaller : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddSingleton<IAppConfig,AppConfig>();


            var b = services.BuildServiceProvider();
            var app = b.GetService<IAppConfig>();
            var tk = app.TokenExpiry;
        }
    }


}