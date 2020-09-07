




using System;

namespace PaymentGateway.Exceptions
{

    public class CreditCardNumberInvalidException : Exception
    {


        public CreditCardNumberInvalidException() : base("Unknown credit card number") { }

        public CreditCardNumberInvalidException(int length, int expected)
        : base($"Length of credit card number not correct | Received = {length} | Minimum length = {expected}")
        { }

        public CreditCardNumberInvalidException(int length, int[] expected)
        : base($"Length of credit card number not correct | Received = {length} | Expected length = {string.Join("or", expected)}")
        { }

    }

    public class PaymentDetailsInvalidException : Exception {
        public PaymentDetailsInvalidException(string msg) : base(msg) {}
    }


}