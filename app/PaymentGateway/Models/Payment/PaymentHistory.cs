



using PaymentGateway.Models.Payment;

namespace PaymentGateway.Models {


    public class PaymentHistory {

        public PaymentDetails PaymentDetails { get; set; }
        public BankTransactionResponse TransactionResponse { get; set; }

    }


}