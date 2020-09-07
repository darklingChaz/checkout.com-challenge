using System.Linq;
using System.Text.RegularExpressions;
using PaymentGateway.Exceptions;

namespace PaymentGateway.Validators
{
    public class CreditCardNumberValidator : IValidate<string>
    {

        private const int minimumCardNumberLength = 13; // Visa


        public void Validate(string creditCardNumber)
        {
            var realNumber = Regex.Replace(creditCardNumber, "[^0-9]", "");

            var length = realNumber.Length;
            if (length < minimumCardNumberLength)
                throw new CreditCardNumberInvalidException(length, minimumCardNumberLength);

            if (IsValidCardNumber(realNumber) == false)
                throw new CreditCardNumberInvalidException();

        }


        private bool IsValidCardNumber(string cardNumber)
        {

            // Luhn Formula
            var fullArray = cardNumber.Select(c => (int)(c - 48)).ToArray();
            var finalMod = fullArray.Last();
            var toCheck = fullArray[0..^1];

            for (var i = toCheck.Length - 1; i >= 0; i -= 2)
            {
                toCheck[i] *= 2;
                if (toCheck[i] > 9)
                    toCheck[i] -= 9;
            }

            var sum = toCheck.Sum() + finalMod;
            return sum % 10 == 0;
        }


    }


}