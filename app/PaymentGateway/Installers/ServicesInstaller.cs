using BankingProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockBank;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;
using PaymentGateway.Translators;
using PaymentGateway.Validators;

namespace PaymentGateway.Installers
{


    public class ServicesInstaller : IInstaller
    {


        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddSingleton<IValidate<PaymentDetails>, PaymentDetailsValidator>();

            services.AddSingleton<IBankProxyTranslator, BankProxyTranslator>();
            services.AddSingleton<IBankProxy, MockBankImpl>();

            services.AddSingleton<ITransactionCache, TransactionCache>();
            services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();

        }
    }


}