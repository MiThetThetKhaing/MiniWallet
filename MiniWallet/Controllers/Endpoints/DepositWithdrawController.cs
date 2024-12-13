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
        private readonly IDepositWithdrawServices _service;

        public DepositWithdrawController(IDepositWithdrawServices service)
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
        public async Task<IActionResult> Deposit(string mobileNo, decimal amount)
        {
            var result = await _service.CreateDeposit(mobileNo, amount);
            return Execute(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(string mobileNo, decimal amount)
        {
            var result = await _service.CreateWithdraw(mobileNo, amount);
            return Execute(result);
        }
    }
}
