




using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PaymentGateway.Installers {

    public interface IInstaller
    {
        
        void ConfigureServices(IServiceCollection services, HostBuilderContext context);

    }


}