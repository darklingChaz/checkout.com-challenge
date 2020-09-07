

using System;
using System.Linq;
using PaymentGateway.Exceptions;
using PaymentGateway.Models.Payment;

namespace PaymentGateway.Validators
{
    public class PaymentDetailsValidator : IValidate<PaymentDetails>
    {
        private readonly CreditCardNumberValidator creditCardNumberValidator = new CreditCardNumberValidator();


        public void Validate(PaymentDetails paymentDetails)
        {
            ValidateExpiry(paymentDetails.ExpiryMonth, paymentDetails.ExpiryYear);

            ValidateCVV(paymentDetails.CVV);
            ValidateAmount(paymentDetails.Amount);

            ValidateCurrency(paymentDetails.Currency);

            // Do this last as it's the most expensive
            creditCardNumberValidator.Validate(paymentDetails.CardNumber);
                    
        }

        private void ValidateExpiry(int month, int year) {

            if(month < 1 || month > 12)
                throw new PaymentDetailsInvalidException($"Invalid expiry month | {month}");

            var d = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
            if(d < DateTime.UtcNow)
                throw new PaymentDetailsInvalidException($"Invalid expiry date | {month}/{year}");

        }

        private void ValidateCVV(string cvv)
        {
            if(string.IsNullOrWhiteSpace(cvv) ||  cvv.Length != 3)
                throw new PaymentDetailsInvalidException("CVV must be 3 digits in length");
        }

        private void ValidateCurrency(string currencyCode)
        {

            string[] codes = new[] {"GBP", "EUR", "CAD", "USD", "NZD", "AUD", "DKK", "HKD"};

            if(string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
                throw new PaymentDetailsInvalidException("Unknown currency supplied");

            if(codes.Contains(currencyCode) == false)
                throw new PaymentDetailsInvalidException("Unknown currency supplied");

        }

        private void ValidateAmount(double amount)
        {
            if(amount < 0)
                throw new PaymentDetailsInvalidException("Amount must be greater than zero");
        }   


    }


}