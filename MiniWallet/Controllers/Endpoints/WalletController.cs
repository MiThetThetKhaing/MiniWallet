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
        private readonly IWalletService _service;

        public WalletController(IWalletService service) 
        { 
            _service = service;
        }

        [HttpGet("{mobileNo}")]
        public async Task<IActionResult> GetWallet(string mobileNo)
        {
            try
            {
                var response = await _service.GetWallet(mobileNo);
                return Execute(response);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CreateWallet(WalletRequestModel newWallet)
        {
            try
            {
                var response = await _service.CreateWallet(newWallet);
                return Execute(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("/changePin/{mobileNo}")]
        public async Task<IActionResult> ChangePin(string mobileNo, ChangePin changePin)
        {
            try
            {
                var response = await _service.ChangePin(mobileNo, changePin.oldPin, changePin.newPin);
                return Execute(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("")]
        public async Task<IActionResult> ChangeMobileNo(string oldMobileNo, string newMobileNo, int pin)
        {
            try
            {
                var response = await _service.ChangeMobileNo(oldMobileNo, newMobileNo, pin);
                return Execute(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("/updateNames/{mobileNo}/{pin}")]
        public async Task<IActionResult> UpdateNames(string mobileNo, int pin, string? userName, string? fullName)
        {
            try
            {
                var response = await _service.ChangeNames(mobileNo, pin, userName, fullName);
                return Execute(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
