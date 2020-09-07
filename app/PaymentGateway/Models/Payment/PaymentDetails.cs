




namespace PaymentGateway.Models.Payment {



    public class PaymentDetails {

        public string CardNumber { get; set; }  
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }

    }


}