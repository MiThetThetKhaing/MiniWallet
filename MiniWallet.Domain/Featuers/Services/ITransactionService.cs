using MiniWallet.Domain.Models;

namespace MiniWallet.Domain.Featuers.Services
{
    public interface ITransactionService
    {
        Task<Result<TransactionResponseModel>> CreateTransaction(string senderMobileNo, string receiverMobileNo, decimal amount, string notes, int Pin);
        Task<Result<TransactionResponseModel>> GetTransaction(string transactionNo);
    }
}