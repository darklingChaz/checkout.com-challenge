


using System;
using System.Threading.Tasks;
using BankingProxy;
using BankingProxy.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services;
using PaymentGateway.Translators;
using PaymentGateway.Validators;

namespace Unit {



    [TestFixture]
    public class PaymentProcessorTests {



        private Mock<IValidate<PaymentDetails>> validator;
        private Mock<IBankProxy> bankProxy;
        private Mock<IBankProxyTranslator> translator;
        private Mock<ILogger<IPaymentProcessorService>> logger;

        private PaymentProcessorService service;


        [SetUp]
        public void Setup() {


            validator = new Mock<IValidate<PaymentDetails>>();
            bankProxy = new Mock<IBankProxy>();
            translator = new Mock<IBankProxyTranslator>();
            logger = new Mock<ILogger<IPaymentProcessorService>>();
            

            service = new PaymentProcessorService(validator.Object, bankProxy.Object, translator.Object, logger.Object);

        }


        [Test]
        public void IfValidationFails_ThrowsException() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            var expectedException = new Exception("ERROR");

            validator.Setup(v => v.Validate(paymentDetails))
                     .Throws(expectedException)
                     .Verifiable();

            // When
            var ex = Assert.ThrowsAsync<Exception>(() => service.Process(paymentDetails));

            // Then
            validator.Verify();
            Assert.AreEqual(expectedException, ex);
        }

        [Test]
        public void IfTranslationFails_CardDetails_ThrowsException() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            var expectedException = new Exception("ERROR");

            translator.Setup(t => t.ToCardDetails(paymentDetails))
                .Throws(expectedException)
                .Verifiable();

            // When
            var ex = Assert.ThrowsAsync<Exception>(() => service.Process(paymentDetails));

            // Then
            translator.Verify();
            Assert.AreEqual(expectedException, ex);
        }

        
        [Test]
        public void IfTranslationFails_TransActionDetails_ThrowsException() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            var expectedException = new Exception("ERROR");

            translator.Setup(t => t.ToCardDetails(paymentDetails))
                .Throws(expectedException)
                .Verifiable();

            // When
            var ex = Assert.ThrowsAsync<Exception>(() => service.Process(paymentDetails));

            // Then
            translator.Verify();
            Assert.AreEqual(expectedException, ex);
        }


        [Test]
        public void IfConnectionToBankError_ThrowsException() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            var expectedException = new Exception("ERROR");

            bankProxy.Setup(t => t.ActionPaymentAsync(It.IsAny<CardDetails>(), It.IsAny<TransactionDetails>()))
                .Throws(expectedException)
                .Verifiable();

            // When
            var ex = Assert.ThrowsAsync<Exception>(() => service.Process(paymentDetails));

            // Then
            bankProxy.Verify();
            Assert.AreEqual(expectedException, ex);
        
        }


        // Problem! If the bank transaction was successful, but error here, how to recover?
        [Test]
        public void IfTranslationToResponseFails_Errors() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
            var expectedException = new Exception("ERROR");

            translator.Setup(t => t.FromTransactionResponse(It.IsAny<TransactionResponse>()))
                .Throws(expectedException)
                .Verifiable();

            // When
            var ex = Assert.ThrowsAsync<Exception>(() => service.Process(paymentDetails));

            // Then
            translator.Verify();
            Assert.AreEqual(expectedException, ex);
        
        }


        [Test]
        public async Task AllIsSuccessful_Response() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();

            var expectedResponse = new BankTransactionResponse { TransactionId = "ID", WasSuccess = true };
            translator.Setup(t => t.FromTransactionResponse(It.IsAny<TransactionResponse>()))
                .Returns(expectedResponse)
                .Verifiable();

            // When
            var response = await service.Process(paymentDetails);

            // Then
            translator.Verify();
            Assert.AreEqual(expectedResponse.TransactionId, response.TransactionId);
            Assert.AreEqual(expectedResponse.WasSuccess, response.WasSuccess);
        
        }


    }


}