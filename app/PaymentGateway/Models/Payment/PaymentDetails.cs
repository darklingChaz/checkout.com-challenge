



using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Models.Payment
{


    [SwaggerTag("Payment Details model")]
    public class PaymentDetails
    {

        [Required]
        public string CardNumber { get; set; }
        [Required]

        public int ExpiryMonth { get; set; }

        [Required]

        public int ExpiryYear { get; set; }
        [Required]

        public string CVV { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Currency { get; set; }


        public PaymentDetails ToMasked()
        {
            return new PaymentDetails {

                CardNumber = CardNumber[^4..].PadLeft(CardNumber.Length - 4, '#'),
                ExpiryMonth = ExpiryMonth,
                ExpiryYear = ExpiryYear,
                CVV = "XXX",
                Amount = Amount,
                Currency = Currency
            };
        }

    }


}