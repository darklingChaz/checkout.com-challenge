






using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Models.Payment;

namespace PaymentGateway.Services {


    public interface ITransactionCache {

        Task Add(BankTransactionResponse transactionResponse, PaymentDetails paymentDetails);

        Task<PaymentHistory> GetPaymentHistory(string transactionId);

    }


    public class TransactionCache : ITransactionCache
    {

        private static readonly object lck = new object();

        private static Dictionary<string, PaymentHistory> cache = new Dictionary<string, PaymentHistory>();
        

        public async Task Add(BankTransactionResponse transactionResponse, PaymentDetails paymentDetails)
        {
            var history = new PaymentHistory {
                PaymentDetails = paymentDetails,
                TransactionResponse = transactionResponse
            };

            lock(lck)
                cache.Add(transactionResponse.TransactionId, history);
            
            await Task.CompletedTask;
        }

        public async Task<PaymentHistory> GetPaymentHistory(string transactionId)
        {
            PaymentHistory history;
            lock(lck)
                history = cache[transactionId];

            return await Task.FromResult(history);
        }
    }

}