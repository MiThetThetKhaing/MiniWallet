using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniWallet.Database.Models;
using MiniWallet.Domain.Models;

namespace MiniWallet.Domain.Featuers.Services
{
    public class WalletService
    {
        private readonly AppDbContext _db;
        private readonly string mobilePattern = @"^(09|01)[0-9]{6,9}$";
        private readonly string pinPattern = @"^[0-9]{6}$";

        public WalletService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<WalletResponseModel>> CreateWallet(WalletRequestModel newWallet)
        {
            Result<WalletResponseModel> response = new Result<WalletResponseModel>();
            
            if(newWallet.WalletUserName.IsNullOrEmpty() || newWallet.WalletUserName.Length < 4)
            {
                response = Result<WalletResponseModel>.ValidationError("User Name can't be empty or shorter than 4 characters.");
                goto Result;
            }
            if(newWallet.FullName.IsNullOrEmpty() || newWallet.FullName.Length < 4)
            {
                response = Result<WalletResponseModel>.ValidationError("Full Name can't be empty or shorter than 4 characters.");
                goto Result;
            }
            if (!Regex.IsMatch(newWallet.MobileNo, mobilePattern))
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number should be \n- 8 digits mimum\n- 11 digits maximum\n- And, should starts from 01 or 09.");
                goto Result;
            }
            if(newWallet.PinCode < 100000)
            {
                response = Result<WalletResponseModel>.ValidationError("Pin Code should have minimum number of 6 digits and should not start with 0(s).");
                goto Result;
            }

            var wallet = new TblWallet
            {
                WalletUserName = newWallet.WalletUserName,
                FullName = newWallet.FullName,
                MobileNo = newWallet.MobileNo,
                PinCode = newWallet.PinCode,
                Balance = 0
            };

            await _db.AddAsync(wallet);
            int result = await _db.SaveChangesAsync();

            if(result > 0)
            {
                WalletResponseModel model = new WalletResponseModel { 
                    Wallet = wallet
                };
                response = Result<WalletResponseModel>.Success(model, "Wallet Created!");
                goto Result;
            }

            response = Result<WalletResponseModel>.SystemError("Internal Server Error!");

            Result:
            return response;
        }

        public async Task<Result<WalletResponseModel>> GetWallet(string mobileNo)
        {
            Result<WalletResponseModel> response = new Result<WalletResponseModel>();

            if (mobileNo.IsNullOrEmpty())
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number can't be null!");
                goto Result;
            }

            var wallet = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);

            if (wallet is not null)
            {
                WalletResponseModel model = new WalletResponseModel
                {
                    Wallet = wallet
                };
                response = Result<WalletResponseModel>.Success(model, "Wallet Found!");
                goto Result;
            }

            response = Result<WalletResponseModel>.NotFound("Wallet Not Found!");

        Result:
            return response;
        }

        public async Task<Result<WalletResponseModel>> ChangePin(string mobileNo, int oldPin, int newPin)
        {
            Result<WalletResponseModel> response = new Result<WalletResponseModel>();

            if (mobileNo.IsNullOrEmpty())
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number can't be null!");
                goto Result;
            }

            var wallet = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);

            if (wallet is null)
            {
                response = Result<WalletResponseModel>.NotFound("Wallet Not Found!");
                goto Result;
            }
            if(wallet.PinCode != oldPin)
            {
                response = Result<WalletResponseModel>.SystemError("Pin Code is not correct!");
                goto Result;
            }

            wallet.PinCode = newPin;

            _db.Entry(wallet).State = EntityState.Modified;
            int result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                WalletResponseModel model = new WalletResponseModel
                {
                    Wallet = wallet
                };
                response = Result<WalletResponseModel>.Success(model, "Pin Changed!");
                goto Result;
            }

            response = Result<WalletResponseModel>.SystemError("Internal Server Error!");

        Result:
            return response;
        }

        public async Task<Result<WalletResponseModel>> ChangeMobileNo(string oldMobileNo, string newMobileNo, int pin)
        {
            Result<WalletResponseModel> response = new Result<WalletResponseModel>();

            if (oldMobileNo.IsNullOrEmpty())
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number can't be null!");
                goto Result;
            }
            if(!Regex.IsMatch(newMobileNo, mobilePattern))
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number should be \n- 8 digits mimum\n- 11 digits maximum\n- And, should starts from 01 or 09.");
                goto Result;
            }

            var wallet = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == oldMobileNo);

            if (wallet is null)
            {
                response = Result<WalletResponseModel>.NotFound("Wallet Not Found!");
                goto Result;
            }
            if (wallet.PinCode != pin)
            {
                response = Result<WalletResponseModel>.SystemError("Pin Code is not correct!");
                goto Result;
            }

            wallet.MobileNo = newMobileNo;

            _db.Entry(wallet).State = EntityState.Modified;
            int result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                WalletResponseModel model = new WalletResponseModel
                {
                    Wallet = wallet
                };
                response = Result<WalletResponseModel>.Success(model, "Mobile No Changed!");
                goto Result;
            }

            response = Result<WalletResponseModel>.SystemError("Internal Server Error!");

        Result:
            return response;
        }

        public async Task<Result<WalletResponseModel>> ChangeNames(string mobileNo, int pin, string userName, string fullName)
        {
            Result<WalletResponseModel> response = new Result<WalletResponseModel>();

            if (mobileNo.IsNullOrEmpty())
            {
                response = Result<WalletResponseModel>.ValidationError("Mobile Number can't be null!");
                goto Result;
            }

            var wallet = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);

            if (wallet is null)
            {
                response = Result<WalletResponseModel>.NotFound("Wallet Not Found!");
                goto Result;
            }
            if (wallet.PinCode != pin)
            {
                response = Result<WalletResponseModel>.SystemError("Pin Code is not correct!");
                goto Result;
            }

            if (!userName.IsNullOrEmpty())
            {
                if (userName.Length < 4)
                {
                    response = Result<WalletResponseModel>.ValidationError("User Name can't be empty or shorter than 4 characters.");
                    goto Result;
                }
                wallet.WalletUserName = userName;
            }
            if (!userName.IsNullOrEmpty())
            {
                if (fullName.IsNullOrEmpty() || fullName.Length < 4)
                {
                    response = Result<WalletResponseModel>.ValidationError("Full Name can't be empty or shorter than 4 characters.");
                    goto Result;
                }
                wallet.FullName = fullName;
            }

            _db.Entry(wallet).State = EntityState.Modified;
            int result = await _db.SaveChangesAsync();

            if (result > 0)
            {
                WalletResponseModel model = new WalletResponseModel
                {
                    Wallet = wallet
                };
                response = Result<WalletResponseModel>.Success(model, "Wallet Updated!");
                goto Result;
            }

            response = Result<WalletResponseModel>.SystemError("Internal Server Error!");

        Result:
            return response;
        }

    }
}
