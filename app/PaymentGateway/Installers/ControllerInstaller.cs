using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Auth;
using PaymentGateway.Models.Auth;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;
using PaymentGateway.Validators;

namespace PaymentGateway.Installers
{


    public class ControllerInstaller
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CustomToken")
                    .AddScheme<CustomTokenAuthenticationOptions, CustomAuthenticationHandler>("CustomToken", opts => {});

            services.AddSingleton<ICredentialTokenManager>(provider =>
            {

                var validCreds = new[] {
                    new AuthCredentials("User1", "Pwd1"),
                    new AuthCredentials("User2", "Pwd2"),
                };

                return new CredentialTokenManager(Constants.DefaultTokenExpiry, validCreds);

            });

        }
    }


}