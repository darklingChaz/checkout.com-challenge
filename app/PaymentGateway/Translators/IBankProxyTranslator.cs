




using System;
using BankingProxy.Models;
using PaymentGateway.Models.Payment;

namespace PaymentGateway.Translators {



    public interface IBankProxyTranslator {

        CardDetails ToCardDetails(PaymentDetails paymentDetails);
        TransactionDetails ToTransactionDetails(PaymentDetails paymentDetails);
        BankTransactionResponse FromTransactionResponse(TransactionResponse transactionResponse);
    }


    public class BankProxyTranslator : IBankProxyTranslator
    {
        public CardDetails ToCardDetails(PaymentDetails paymentDetails)
        {
            return new CardDetails {
                CardNumber = paymentDetails.CardNumber,
                Month = paymentDetails.ExpiryMonth, 
                Year = paymentDetails.ExpiryYear,
                CVV = paymentDetails.CVV
            };
        }

        public TransactionDetails ToTransactionDetails(PaymentDetails paymentDetails)
        {
            return new TransactionDetails {
                Amount = paymentDetails.Amount, 
                Currency = paymentDetails.Currency
            };
        }


        public BankTransactionResponse FromTransactionResponse(TransactionResponse transactionResponse) {
            
            return new BankTransactionResponse {
                TransactionId = transactionResponse.TransactionId,
                StatusCode = transactionResponse.StatusCode,
                WasSuccess = transactionResponse.StatusCode == TransactionStatusCodes.Success
            };
        }

    }


}