using MiniWallet.Database.Models;
using MiniWallet.Domain.Models;

namespace MiniWallet.Domain.Featuers.Services
{
    public interface IDepositWithdrawServices
    {
        Task<Result<DepositWithdrawResponseModel>> CreateDeposit(TblDepositWithdraw deposit);
        Task<Result<DepositWithdrawResponseModel>> CreateWithdraw(TblDepositWithdraw withdraw);
        Task<Result<DepositWithdrawResponseModel>> GetAllDepositWithdraw(string mobileNo);
    }
}