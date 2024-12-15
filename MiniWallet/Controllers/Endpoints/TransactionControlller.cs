using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniWallet.Domain.Featuers.Services;
using MiniWallet.Domain.Models;

namespace MiniWallet.Api.Controllers.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionControlller : BaseController
    {
        private readonly ITransactionService _service;

        public TransactionControlller(ITransactionService service)
        {
            _service = service;
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferRequestModel transfer)
        {
            var model = await _service.CreateTransaction(
                transfer.senderMobileNo, transfer.receiverMobileNo, transfer.amount, transfer.notes, transfer.Pin);
            return Execute(model);
        }

        [HttpGet("TransactionDetail")]
        public async Task<IActionResult> GetTransaction(string transactionNo)
        {
            var model = await _service.GetTransaction(transactionNo);
            return Execute(model);
        }
    }
}
