






using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using PaymentGateway.Exceptions;
using PaymentGateway.Models.Payment;
using PaymentGateway.Validators;

namespace Unit {


    [TestFixture]
    public class PaymentDetailsValidatorTests {


        private PaymentDetailsValidator validator = new PaymentDetailsValidator();


        [Test]
        public void CorrectDetailsPass() {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();

            // When
            validator.Validate(details);
        
            // Then
            Assert.Pass("No issues");
        
        }


        [TestCase(-5)]
        [TestCase(0)]
        [TestCase(13)]
        public void InvalidMonthFails(int month) {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.ExpiryMonth = month;


            // When
            var ex = Assert.Throws<PaymentDetailsInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("month"));
        
        }


        [TestCase]
        public void YearMonthBeforeCurrentFails() {
        
            var rnd = new Random();
            var expiredDate = new DateTime(2020, 2, DateTime.DaysInMonth(2020, 2)).Subtract(TimeSpan.FromDays(rnd.Next(31, 600)));

            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.ExpiryMonth = expiredDate.Month;
            details.ExpiryYear = expiredDate.Year;

            // When
            var ex = Assert.Throws<PaymentDetailsInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("expiry date"));
        
        }


        [TestCase("12")]
        [TestCase("564434")]
        [TestCase("")]
        [TestCase("XXX")]
        [TestCase(null)]
        public void InvalidCVV(string cvv) {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.CVV = cvv;

            // When
            var ex = Assert.Throws<PaymentDetailsInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("cvv"));
        
        }

        
        [TestCase]
        public void AmountLessThanZeroFails() {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.Amount = -20.0;

            // When
            var ex = Assert.Throws<PaymentDetailsInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("amount"));
        
        }


        [TestCase("12")]
        [TestCase("XXX")]
        [TestCase("XX")]
        [TestCase("XXXXXX")]
        [TestCase("")]
        [TestCase(null)]
        public void InvalidCurrency(string currency) {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.Currency = currency;


            // When
            var ex = Assert.Throws<PaymentDetailsInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("currency"));
        
        }


        // for completion, full credit card validation number tests exists
        [Test]
        public void InvalidCreditCardNumber() {
        
            // Given
            var details = PaymentDetailsGenerator.GetValidDetails();
            details.CardNumber = "4012888888881882"; // invalid by 1
        
            // When
            var ex = Assert.Throws<CreditCardNumberInvalidException>(() => validator.Validate(details));
        
            // Then
            Assert.IsTrue(ex.Message.ToLower().Contains("unknown credit card number"));
        
        }


    }

}