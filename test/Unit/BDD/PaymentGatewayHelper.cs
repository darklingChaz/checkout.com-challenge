



using System;
using System.Collections.Generic;
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

        public const string HistoryUrl = "/api/v1/payment/history";



        public PaymentGatewayHelper() : base("http://localhost:5000")
        { }


        public async Task StartGateway()
        {

            var host = Program.CreateHostBuilder(new string[0]);
            host.UseEnvironment("Development");
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

        public async Task<AuthToken> GetAuthToken(AuthCredentials authCreds)
        {
            return await ExecuteHttpRequest<AuthCredentials, AuthToken>(HttpMethod.Post, AuthTokenUrl, authCreds);
        }

        public async Task<BankTransactionResponse> SubmitPayment(PaymentDetails paymentDetails, string authToken)
        {
            var headers = new Dictionary<string,string>().WithHeader("Authorization", authToken);
            return await ExecuteHttpRequest<PaymentDetails, BankTransactionResponse>(HttpMethod.Post, PaymentUrl, paymentDetails, headers);
        }

        internal async Task<PaymentHistory> GetPaymentHistory(string transactionId, string authToken)
        {
            var headers = new Dictionary<string,string>().WithHeader("Authorization", authToken);
            return await ExecuteHttpRequest<PaymentHistory>(HttpMethod.Get, HistoryUrl + $"/{transactionId}", headers);
        }
    }


}