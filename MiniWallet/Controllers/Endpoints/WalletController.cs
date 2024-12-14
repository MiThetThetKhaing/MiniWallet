using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniWallet.Domain.Featuers.Services;
using MiniWallet.Domain.Models;

namespace MiniWallet.Api.Controllers.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : BaseController
    {
        private readonly WalletService _service;

        public WalletController(WalletService service) 
        { 
            _service = service;
        }

        [HttpGet("{mobileNo}")]
        public async Task<IActionResult> GetWallet(string mobileNo)
        {
            var response = await _service.GetWallet(mobileNo);
            return Execute(response);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateWallet(WalletRequestModel newWallet)
        {
            var response = await _service.CreateWallet(newWallet);
            return Execute(response);
        }

        [HttpPatch("{mobileNo}")]
        public async Task<IActionResult> ChangePin(string mobileNo, int oldPin, int newPin)
        {
            var response = await _service.ChangePin(mobileNo, oldPin, newPin);
            return Execute(response);
        }

        [HttpPatch("")]
        public async Task<IActionResult> ChangeMobileNo(string oldMobileNo, string newMobileNo, int pin)
        {
            var response = await _service.ChangeMobileNo(oldMobileNo, newMobileNo, pin);
            return Execute(response);
        }

        [HttpPatch("{mobileNo}")]
        public async Task<IActionResult> UpdateNames(string mobileNo, int pin, string? userName, string? fullName)
        {
            var response = await _service.ChangeNames(mobileNo, pin, userName, fullName);
            return Execute(response);
        }
    }
}
