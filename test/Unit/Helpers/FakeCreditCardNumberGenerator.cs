



using System;
using PaymentGateway.Models.Payment;

namespace Unit
{


    public static class PaymentDetailsGenerator
    {


        public static PaymentDetails GetValidDetails() {

            var d = DateTime.UtcNow;

            return new PaymentDetails {
                CardNumber = GetValidCreditCardNumber(),
                ExpiryMonth = 12,
                ExpiryYear = d.Year + 1,
                CVV = "123",
                Currency = "GBP",
                Amount = 62.30
            };

        }


        public static string GetValidCreditCardNumber()
        {

            var cards = new[] {"4339172096975645",
                                "4532756784548980",
                                "4204274523862865924",
                                "4556737586899855",
                                "4012888888881881",
                                "4556737586899855",
                                "5290425604312113",
                                "5334360322421227",
                                "5127046809210149",
                                "376831198750675",
                                "379117789912495",
                                "379431472100171",
                                "6011799966122856",
                                "6011801377890905",
                                "6011423902667460973",
                                "3543644377524155",
                                "3540965478503290",
                                "3542713605465018986",
                                "5415388666773533",
                                "5596808584908300",
                                "5518631514705769",
                                "30076596219828",
                                "30236832075653",
                                "30306541753740",
                                "36235803169846",
                                "36526105097804",
                                "36773027611276",
                                "5038904755412267",
                                "6761793103563769",
                                "0604222421725037",
                                "4913568174005136",
                                "4913041181747728",
                                "4508735727117715",
                                "6382020793191852",
                                "6389644534559078",
                                "6375449855262146"
                            };

            var rnd = new Random();
            var i = rnd.Next(0, cards.Length - 1);
            return cards[i];

        }

    }


}