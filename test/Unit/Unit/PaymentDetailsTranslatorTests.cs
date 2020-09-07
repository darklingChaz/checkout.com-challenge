



using System;
using BankingProxy.Models;
using NUnit.Framework;
using PaymentGateway.Translators;

namespace Unit {



    [TestFixture]
    public class PaymentDetailsTranslatorTests {




        private BankProxyTranslator translator = new BankProxyTranslator();



        [Test]
        public void CanTranslate_ToCardDetails() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            // When
            var cardDetails = translator.ToCardDetails(paymentDetails);

            // Then
            Assert.AreEqual(paymentDetails.CardNumber, cardDetails.CardNumber, "CardNumber");
            Assert.AreEqual(paymentDetails.ExpiryMonth, cardDetails.Month, "Month");
            Assert.AreEqual(paymentDetails.ExpiryYear, cardDetails.Year, "Year");
            Assert.AreEqual(paymentDetails.CVV, cardDetails.CVV, "CVV");
        
        }


        
        [Test]
        public void CanTranslate_ToTransactionDetails() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            // When
            var transactionDetails = translator.ToTransactionDetails(paymentDetails);

            // Then
            Assert.AreEqual(paymentDetails.Amount, transactionDetails.Amount, "Amount");
            Assert.AreEqual(paymentDetails.Currency, transactionDetails.Currency, "Currency");
        
        }


        [TestCase(TransactionStatusCodes.Failed, false)]
        [TestCase(TransactionStatusCodes.Success, true)]
        public void CanTranslate_FromTransactionResponse_Success(string statusCode, bool expectedSuccess) {
        
            // Given
            var transactionResponse = new TransactionResponse { TransactionId = Guid.NewGuid().ToString(), StatusCode = statusCode };

            // When
            var bankTransaction = translator.FromTransactionResponse(transactionResponse);
        
            // Then
            Assert.AreEqual(transactionResponse.TransactionId, bankTransaction.TransactionId);
            Assert.AreEqual(expectedSuccess, bankTransaction.WasSuccess);
        
        }


    }


}