





using System;
using System.Threading.Tasks;
using BankingProxy;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models.Payment;
using PaymentGateway.Translators;
using PaymentGateway.Validators;

namespace PaymentGateway.Services {


    public interface IPaymentProcessorService {


        Task<BankTransactionResponse> Process(PaymentDetails paymentDetails);

    }

    public class PaymentProcessorService : IPaymentProcessorService
    {

        private readonly ILogger<IPaymentProcessorService> logger;
        private readonly IValidate<PaymentDetails> paymentDetailsValidator;
        private readonly IBankProxy bankProxy;
        private readonly IBankProxyTranslator translator;

        public PaymentProcessorService(IValidate<PaymentDetails> paymentDetailsValidator, IBankProxy bankProxy, IBankProxyTranslator translator, ILogger<IPaymentProcessorService> logger)
        {
            this.paymentDetailsValidator = paymentDetailsValidator;
            this.bankProxy = bankProxy;
            this.translator = translator;
            this.logger = logger;
        }


        public async Task<BankTransactionResponse> Process(PaymentDetails paymentDetails)
        {
            
            try{

                paymentDetailsValidator.Validate(paymentDetails);

                var cardDetails = translator.ToCardDetails(paymentDetails);
                var transactionDetails = translator.ToTransactionDetails(paymentDetails);

                var transactionResponse = await bankProxy.ActionPaymentAsync(cardDetails, transactionDetails);

                return translator.FromTransactionResponse(transactionResponse);

            } catch(Exception e) {

                logger.LogError(e, "Error while trying process transaction with bank");
                throw e;

            }

        }

    }

}