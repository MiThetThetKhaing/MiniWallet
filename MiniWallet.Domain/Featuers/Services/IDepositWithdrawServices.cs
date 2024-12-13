using MiniWallet.Domain.Models;

namespace MiniWallet.Domain.Featuers.Services
{
    public interface IDepositWithdrawServices
    {
        Task<Result<DepositWithdrawResponseModel>> CreateDeposit(string mobileNo, decimal amount);
        Task<Result<DepositWithdrawResponseModel>> CreateWithdraw(string mobileNo, decimal amount);
        Task<Result<DepositWithdrawResponseModel>> GetDepositWithdraw(string txnNo);
    }
}