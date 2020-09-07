





using Swashbuckle.AspNetCore.Annotations;

namespace PaymentGateway.Models.Payment
{

    [SwaggerTag("Transaction response")]
    public class BankTransactionResponse
    {
        public string TransactionId { get; set; }
        public string StatusCode {get;set;}
        public bool WasSuccess { get; set; }

    }

}