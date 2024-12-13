using Microsoft.AspNetCore.Mvc;
using MiniWallet.Database.Models;
using MiniWallet.Domain.Models;

namespace MiniWallet.Api.Controllers.Endpoints
{
    public interface IDepositWithdrawController
    {
        Task<IActionResult> Deposit(DepositWithdrawRequestModel deposit);
        Task<IActionResult> GetDepositWithdraw(string mobileNo);
        Task<IActionResult> Withdraw(TblDepositWithdraw withdraw);
    }
}