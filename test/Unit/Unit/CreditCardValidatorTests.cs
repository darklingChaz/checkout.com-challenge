




using System;
using NUnit.Framework;
using PaymentGateway.Exceptions;
using PaymentGateway.Validators;

namespace Unit {


    [TestFixture]
    public class CreditCardValidatorTests {



        private readonly CreditCardNumberValidator validator = new CreditCardNumberValidator();


        [Test]
        public void CreditCardNumberTooShort() {
        
            // Given
            var creditCardNumber = "33445";
        
            // When
            

            // Then
            Assert.Throws<CreditCardNumberInvalidException>(() => validator.Validate(creditCardNumber));
        
        }

        [Test]
        public void CreditCardNumberTooShortWithoutNumericCharacters() {
        
            // Given
            var creditCardNumber = "33445-55564-44";
        
            // When
        

            // Then
            Assert.Throws<CreditCardNumberInvalidException>(() => validator.Validate(creditCardNumber));
        
        }


        [Test]
        public void CreditCardNumberIsInvalid() {
        
            // Given
            var creditCardNumber = "00000-00000-00003";
        
            // When
            

            // Then
            Assert.Throws<CreditCardNumberInvalidException>(() => validator.Validate(creditCardNumber));
        
        }


        // taken from https://www.freeformatter.com/credit-card-number-generator-validator.html
        [TestCase("4339172096975645")]
        [TestCase("4532756784548980")]
        [TestCase("4204274523862865924")]
        [TestCase("4556737586899855")]
        [TestCase("4012888888881881")]
        [TestCase("4556737586899855")]
        [TestCase("5290425604312113")]
        [TestCase("5334360322421227")]
        [TestCase("5127046809210149")]
        [TestCase("376831198750675")]
        [TestCase("379117789912495")]
        [TestCase("379431472100171")]
        [TestCase("6011799966122856")]
        [TestCase("6011801377890905")]
        [TestCase("6011423902667460973")]
        [TestCase("3543644377524155")]
        [TestCase("3540965478503290")]
        [TestCase("3542713605465018986")]
        [TestCase("5415388666773533")]
        [TestCase("5596808584908300")]
        [TestCase("5518631514705769")]
        [TestCase("30076596219828")]
        [TestCase("30236832075653")]
        [TestCase("30306541753740")]
        [TestCase("36235803169846")]
        [TestCase("36526105097804")]
        [TestCase("36773027611276")]
        [TestCase("5038904755412267")]
        [TestCase("6761793103563769")]
        [TestCase("0604222421725037")]
        [TestCase("4913568174005136")]
        [TestCase("4913041181747728")]
        [TestCase("4508735727117715")]
        [TestCase("6382020793191852")]
        [TestCase("6389644534559078")]
        [TestCase("6375449855262146")]
        public void ValidCreditCards(string creditCardNumber) {
        
            // No fail is a pass
            validator.Validate(creditCardNumber);
        
        }



    }

}