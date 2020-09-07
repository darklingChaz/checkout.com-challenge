



using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PaymentGateway;
using PaymentGateway.Models;
using PaymentGateway.Models.Auth;
using PaymentGateway.Models.Payment;
using Unit.Helpers;

namespace Unit.BDD
{


    public class PaymentGatewayHelper : ApiQueryHelper
    {

        private IHost paymentGatewayHost;



        public const string InfoUrl = "/info";
        public const string AuthTokenUrl = "/api/v1/payment/authtoken";
        public const string PaymentUrl = "/api/v1/payment/submit";



        public PaymentGatewayHelper() : base("https://localhost:5001")
        { }


        public async Task StartGateway()
        {

            var host = Program.CreateHostBuilder(new string[0]);
            paymentGatewayHost = host.Build();
            await paymentGatewayHost.StartAsync();
        }

        public async Task StopGateway()
        {

            if (paymentGatewayHost != null)
                await paymentGatewayHost.StopAsync();

            paymentGatewayHost = null;
        }

        public async Task<ApplicationStatus> GetApplicationStatus()
        {

            return await ExecuteHttpRequest<ApplicationStatus>(HttpMethod.Get, InfoUrl);

        }

        internal async Task<AuthToken> GetAuthToken(AuthCredentials authCreds)
        {
            return await ExecuteHttpRequest<AuthCredentials, AuthToken>(HttpMethod.Post, AuthTokenUrl, authCreds);
        }

        internal Task SubmitPayment(PaymentDetails paymentDetails, object p)
        {
            throw new NotImplementedException();
        }
    }


}