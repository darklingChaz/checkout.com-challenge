




using System;
using System.Threading.Tasks;
using BankingProxy;
using BankingProxy.Models;

namespace MockBank
{


    public class MockBankImpl : IBankProxy
    {
        public async Task<TransactionResponse> ActionPaymentAsync(CardDetails cardDetails, TransactionDetails transactionDetails)
        {
            
            var response = new TransactionResponse { TransactionId = Guid.NewGuid().ToString(), StatusCode = TransactionStatusCodes.Success };
            return await Task.FromResult(response);

        }
    }


}