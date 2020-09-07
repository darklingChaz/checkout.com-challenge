





using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankingProxy.Models;
using NUnit.Framework;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;

namespace Unit {


    [TestFixture]
    public class TransactionCacheTests {




        private TransactionCache cache;


        [SetUp]
        public void Setup() {
            cache = new TransactionCache();
        }



        [Test]
        public async Task CanAddToAndGetFromCache() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails().ToMasked();
            var transactionResponse = new BankTransactionResponse { TransactionId = Guid.NewGuid().ToString(), StatusCode = TransactionStatusCodes.Success, WasSuccess = true  };
        
            // When
            await cache.Add(transactionResponse, paymentDetails);

            var history = await cache.GetPaymentHistory(transactionResponse.TransactionId);

            // Then
            Assert.AreEqual(paymentDetails.CardNumber, history.PaymentDetails.CardNumber);
            Assert.AreEqual(paymentDetails.ExpiryMonth, history.PaymentDetails.ExpiryMonth);
            Assert.AreEqual(paymentDetails.ExpiryYear, history.PaymentDetails.ExpiryYear);
            Assert.AreEqual(paymentDetails.CVV, history.PaymentDetails.CVV);
            Assert.AreEqual(paymentDetails.Amount, history.PaymentDetails.Amount);
            Assert.AreEqual(paymentDetails.Currency, history.PaymentDetails.Currency);

            Assert.AreEqual(transactionResponse.TransactionId, history.TransactionResponse.TransactionId);
            Assert.AreEqual(transactionResponse.StatusCode, history.TransactionResponse.StatusCode);
            Assert.AreEqual(transactionResponse.WasSuccess, history.TransactionResponse.WasSuccess);
        
        }

        [Test]
        public void IfTransactionNotInCache_ThrowsException() {
        
            // Given
            var unknownTransactionId = "UNKNOWN";

            // When
        
            // Then
            Assert.ThrowsAsync<KeyNotFoundException>(() => cache.GetPaymentHistory(unknownTransactionId));
        
        }

        [Test]
        public async Task IfTransactionAlreadyExists_ThrowsException() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails().ToMasked();
            var transactionResponse = new BankTransactionResponse { TransactionId = Guid.NewGuid().ToString(), StatusCode = TransactionStatusCodes.Success, WasSuccess = true  };
        
            // When
            await cache.Add(transactionResponse, paymentDetails);
        
            // Then
            Assert.ThrowsAsync<ArgumentException>(() => cache.Add(transactionResponse, paymentDetails));
        
        }


    }

}