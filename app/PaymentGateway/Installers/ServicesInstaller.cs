using System;
using BankingProxy;
using Microsoft.Extensions.Configuration;
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
            services.AddSingleton<ITransactionCache, TransactionCache>();
            services.AddSingleton<IPaymentProcessorService, PaymentProcessorService>();

            // this would be more elegant
            if(context.HostingEnvironment.IsProduction()) {
                services.AddSingleton<IBankProxy, MockBankRestImpl>(provider => {
                    
                    var appConfig = provider.GetRequiredService<IAppConfig>();

                    return new MockBankRestImpl(new Uri(appConfig.BankingApiEndpoint));
                });
            }
            else
                services.AddSingleton<IBankProxy, MockBankImpl>();



        }
    }


}