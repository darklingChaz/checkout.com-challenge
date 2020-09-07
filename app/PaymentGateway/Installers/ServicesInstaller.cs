using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;
using PaymentGateway.Validators;

namespace PaymentGateway.Installers
{


    public class ServicesInstaller : IInstaller
    {


        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddSingleton<IValidate<PaymentDetails>, PaymentDetailsValidator>();
            services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();

        }
    }


}