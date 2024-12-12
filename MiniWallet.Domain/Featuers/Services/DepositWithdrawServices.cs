﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MiniWallet.Database.Models;
using MiniWallet.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Featuers.Services
{
    public class DepositWithdrawServices : IDepositWithdrawServices
    {
        private readonly AppDbContext _db;

        public DepositWithdrawServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<DepositWithdrawResponseModel>> GetAllDepositWithdraw(string mobileNo)
        {
            try
            {
                var account = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);
                var mobile = await _db.TblDepositWithdraws.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);

                if (account is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile number doesn't have wallet account.");
                }
                if (mobile is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile doesn't have any transaction.");
                }

                var list = await _db.TblDepositWithdraws.AsNoTracking().ToListAsync();
                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdrawList = list
                };

                return Result<DepositWithdrawResponseModel>.Success(result, "Here are Transaction for your mobile number.");
            }
            catch (Exception ex)
            {
                return Result<DepositWithdrawResponseModel>.SystemError(ex.Message);
            }
        }

        public async Task<Result<DepositWithdrawResponseModel>> CreateDeposit(TblDepositWithdraw deposit)
        {
            try
            {
                var mobile = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == deposit.MobileNo);

                #region "Validation"
                if (mobile is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile number doesn't have wallet account.");
                }
                if (deposit.Amount <= 0)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Please Enter valid amount.");
                }
                #endregion

                #region "Balance Calculation"
                mobile.Balance += deposit.Amount;
                _db.TblWallets.Update(mobile);
                #endregion

                #region "deposit"
                deposit.TransactionType = "CR";
                deposit.Date = DateTime.Now;
                deposit.No = "DPT-" + deposit.Date.ToString("yyyyMMdd") + "-" + deposit.Id.ToString("D4"); //DPT-20241212-001

                await _db.TblDepositWithdraws.AddAsync(deposit);
                await _db.SaveChangesAsync();
                #endregion

                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdraw = deposit
                };

                return Result<DepositWithdrawResponseModel>.Success(result, "Cash Deposit is successfully completed.");
            }
            catch (Exception ex)
            {
                return Result<DepositWithdrawResponseModel>.SystemError(ex.Message);
            }
        }

        public async Task<Result<DepositWithdrawResponseModel>> CreateWithdraw(TblDepositWithdraw withdraw)
        {
            try
            {
                var mobile = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == withdraw.MobileNo);

                #region "Validation"
                if (mobile is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile number doesn't have wallet account.");
                }
                if (withdraw.Amount <= 0)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Please Enter valid amount.");
                }
                if (mobile.Balance - withdraw.Amount < 10000)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Insufficient Balance.");
                }
                #endregion

                #region "Balance Calculation"
                mobile.Balance -= withdraw.Amount;
                _db.TblWallets.Update(mobile);
                #endregion

                #region "Withdraw"
                withdraw.TransactionType = "DR";
                withdraw.Date = DateTime.Now;
                withdraw.No = "WTH-" + withdraw.Date.ToString("yyyyMMdd") + "-" + withdraw.Id.ToString("D4"); //DPT-20241212-001

                await _db.TblDepositWithdraws.AddAsync(withdraw);
                await _db.SaveChangesAsync();
                #endregion

                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdraw = withdraw
                };

                return Result<DepositWithdrawResponseModel>.Success(result, "Cash Withdraw is successfully completed.");
            }
            catch (Exception ex)
            {
                return Result<DepositWithdrawResponseModel>.SystemError(ex.Message);
            }
        }
    }
}
