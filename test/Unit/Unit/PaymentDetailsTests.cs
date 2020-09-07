using NUnit.Framework;

namespace Unit
{


    [TestFixture]
    public class PaymentDetailsTests {


        
        [Test]
        public void CanMaskPaymentDetails() {
        
            // Given
            var paymentDetails = PaymentDetailsGenerator.GetValidDetails();
        
            // When
            var masked = paymentDetails.ToMasked();
        
            // Then
            Assert.IsTrue(masked.CardNumber.EndsWith(paymentDetails.CardNumber[^4..]));
            Assert.AreEqual(paymentDetails.ExpiryMonth, masked.ExpiryMonth);
            Assert.AreEqual(paymentDetails.ExpiryYear, masked.ExpiryYear);
            Assert.AreEqual("XXX", masked.CVV);

            Assert.AreEqual(paymentDetails.Amount, masked.Amount);
            Assert.AreEqual(paymentDetails.Currency, masked.Currency);

        }



    }


}