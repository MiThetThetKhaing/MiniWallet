using MiniWallet.Domain.Models;

namespace MiniWallet.Domain.Featuers.Services
{
    public interface IWalletService
    {
        Task<Result<WalletResponseModel>> ChangeMobileNo(string oldMobileNo, string newMobileNo, int pin);
        Task<Result<WalletResponseModel>> ChangeNames(string mobileNo, int pin, string? userName, string? fullName);
        Task<Result<WalletResponseModel>> ChangePin(string mobileNo, int oldPin, int newPin);
        Task<Result<WalletResponseModel>> CreateWallet(WalletRequestModel newWallet);
        Task<Result<WalletResponseModel>> GetWallet(string mobileNo);
    }
}