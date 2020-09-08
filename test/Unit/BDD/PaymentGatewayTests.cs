



using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.Models;
using PaymentGateway.Models.Auth;
using PaymentGateway.Models.Payment;

namespace Unit.BDD
{


    [TestFixture]
    public class PaymentGatewayTests

    {


        private PaymentGatewayHelper paymentGatewayHelper;


        private static readonly AuthCredentials[] validCreds = {

            new AuthCredentials("User1", "Pwd1"),
            new AuthCredentials("User2", "Pwd2"),

        };



        [SetUp]
        public async Task Setup()
        {

            paymentGatewayHelper = new PaymentGatewayHelper();
            await paymentGatewayHelper.StartGateway();

        }

        [TearDown]
        public async Task Teardown()
        {

            await paymentGatewayHelper.StopGateway();

        }




        [Test]
        public async Task CanGetAuthToken()
        {

            // Given
            var authCreds = validCreds[0];

            // When
            var authToken = await paymentGatewayHelper.GetAuthToken(authCreds);

            // Then
            Assert.IsNotNull(authToken);

        }


        [Test]
        public async Task InvalidCreds_TryGetAuthToken_Returns401()
        {

            // Given
            var invalidCreds = new AuthCredentials("UNKNOWN", "NOT SET");

            // When
            var response = await paymentGatewayHelper.ExecuteHttpRequest(HttpMethod.Post, PaymentGatewayHelper.AuthTokenUrl, invalidCreds);

            // Then
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

        }



        [Test]
        public async Task WithOutToken_CanNotSubmitPayment()
        {

            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            // When
            var response = await paymentGatewayHelper.ExecuteHttpRequest(HttpMethod.Post, PaymentGatewayHelper.PaymentUrl, paymentDetails);

            // Then
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

        }

        [Test]
        public async Task InvalidPaymentDetails_ReturnsBadRequest()
        {

            // Given
            var authToken = await paymentGatewayHelper.GetAuthToken(validCreds[0]);
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            paymentDetails.ExpiryMonth = -1;

            // When
            var headers = new Dictionary<string, string>() {
                { "Authorization", authToken.Token }
            };
            var response = await paymentGatewayHelper.ExecuteHttpRequest(HttpMethod.Post, PaymentGatewayHelper.PaymentUrl, paymentDetails, headers);

            // Then
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }


        [Test]
        public async Task ValidPaymentDetails_GetsTransactionCodeFromBank()
        {

            // Given
            var authToken = await paymentGatewayHelper.GetAuthToken(validCreds[0]);
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            // When
            var transactionResponse = await paymentGatewayHelper.SubmitPayment(paymentDetails, authToken.Token);

            // Then
            Assert.IsNotNull(transactionResponse);
            

        }


        [Test]
        public async Task WithoutToken_CanNotGetHistory() {
        
            // Given
            var transactionId = Guid.NewGuid().ToString();

            // When
            var response = await paymentGatewayHelper.ExecuteHttpRequest(HttpMethod.Get, PaymentGatewayHelper.HistoryUrl + $"/{transactionId}");

            // Then
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        
        }


        [Test]
        public async Task CanGetPaymentHistory() {

            // Given
            var authToken = await paymentGatewayHelper.GetAuthToken(validCreds[0]);
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            var transactionResponse = await paymentGatewayHelper.SubmitPayment(paymentDetails, authToken.Token);

            // When
            var history = await paymentGatewayHelper.GetPaymentHistory(transactionResponse.TransactionId, authToken.Token);

            // Then
            var masked = paymentDetails.ToMasked();
            Assert.AreEqual(masked.CardNumber, history.PaymentDetails.CardNumber);
            Assert.AreEqual(masked.ExpiryMonth, history.PaymentDetails.ExpiryMonth);
            Assert.AreEqual(masked.ExpiryYear, history.PaymentDetails.ExpiryYear);
            Assert.AreEqual(masked.CVV, history.PaymentDetails.CVV);
            Assert.AreEqual(masked.Amount, history.PaymentDetails.Amount);
            Assert.AreEqual(masked.Currency, history.PaymentDetails.Currency);

            Assert.AreEqual(transactionResponse.TransactionId, history.TransactionResponse.TransactionId);
            Assert.AreEqual(transactionResponse.StatusCode, history.TransactionResponse.StatusCode);
            Assert.AreEqual(transactionResponse.WasSuccess, history.TransactionResponse.WasSuccess);

        }




        [Test]
        public async Task CanSubmitManyRequests() {
        
            // Given
            var authToken = await paymentGatewayHelper.GetAuthToken(validCreds[0]);

            var transactionIds = new List<string>();
            var submit = new Func<string,Task<string>>(async authToken => {
                
                var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
                var transactionResponse = await paymentGatewayHelper.SubmitPayment(paymentDetails, authToken);
                return transactionResponse.TransactionId;
                
            } );

            for (int i = 0; i < 40; i++)
                transactionIds.Add(await submit(authToken.Token));

            // Then
            foreach (var id in transactionIds){
                var hist = await paymentGatewayHelper.GetPaymentHistory(id, authToken.Token);
                Assert.AreEqual(id, hist.TransactionResponse.TransactionId);
                Console.WriteLine($"${hist.TransactionResponse.TransactionId} | ${hist.TransactionResponse.StatusCode}");
            }
        }


    }

}