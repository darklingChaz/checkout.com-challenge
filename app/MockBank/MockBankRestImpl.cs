




using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BankingProxy;
using BankingProxy.Models;
using Newtonsoft.Json;

namespace MockBank
{
    public class MockBankRestImpl : IBankProxy
    {

        private readonly HttpClient client = new HttpClient();
        private readonly Uri bankingEndpoint;

        public MockBankRestImpl(Uri bankingEndpoint)
        {
            this.bankingEndpoint = bankingEndpoint;
        }

        public async Task<TransactionResponse> ActionPaymentAsync(CardDetails cardDetails, TransactionDetails transactionDetails)
        {
            
            var url =$"{bankingEndpoint}actionpayment";
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var details = new DetailsWrapper{ CardDetails = cardDetails, TransactionDetails = transactionDetails };
            var content = JsonConvert.SerializeObject(details);
            var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var responseAsJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TransactionResponse>(responseAsJson);
            }

            throw new Exception($"ERROR WHEN CONNECTING TO BANK | {response.StatusCode} | {response.ReasonPhrase}");

        }
    }


}