




using System.Threading.Tasks;
using BankingProxy.Models;

namespace BankingProxy {


    public interface IBankProxy {


        Task<TransactionResponse> ActionPaymentAsync(CardDetails cardDetails, TransactionDetails transactionDetails);


    }


}