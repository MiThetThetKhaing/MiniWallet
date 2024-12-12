using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniWallet.Database.Models;
using MiniWallet.Domain.Featuers.Services;
using MiniWallet.Domain.Models;

namespace MiniWallet.Api.Controllers.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositWithdrawController : BaseController
    {
        private readonly DepositWithdrawServices _service;

        public DepositWithdrawController(DepositWithdrawServices service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetDepositWithdraw(string mobileNo)
        {
            var result = await _service.GetAllDepositWithdraw(mobileNo);
            return Execute(result);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(TblDepositWithdraw deposit)
        {
            var result = await _service.CreateDeposit(deposit);
            return Execute(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(TblDepositWithdraw withdraw)
        {
            var result = await _service.CreateWithdraw(withdraw);
            return Execute(result);
        }
    }
}
