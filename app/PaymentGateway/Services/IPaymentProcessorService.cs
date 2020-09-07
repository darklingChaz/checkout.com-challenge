





using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Payment;
using PaymentGateway.Validators;

namespace PaymentGateway.Services {


    public interface IPaymentProcessorService {


        Task<BankTransactionResponse> Process(PaymentDetails paymentDetails);

    }

    public class PaymentProcessorService : IPaymentProcessorService
    {

        private readonly ILogger<PaymentProcessorService> logger;
        private readonly IValidate<PaymentDetails> paymentDetailsValidator;
        


        public PaymentProcessorService(IValidate<PaymentDetails> paymentDetailsValidator, ILogger<PaymentProcessorService> logger)
        {
            this.paymentDetailsValidator = paymentDetailsValidator;
            this.logger = logger;
        }


        public async Task<BankTransactionResponse> Process(PaymentDetails paymentDetails)
        {
            
            paymentDetailsValidator.Validate(paymentDetails);

            return await Task.FromResult(default(BankTransactionResponse));
        }

    }

}