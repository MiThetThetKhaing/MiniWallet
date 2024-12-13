using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<DepositWithdrawResponseModel>> GetDepositWithdraw(string txnNo)
        {
            try
            {
                var transactionNo = await _db.TblDepositWithdraws.AsNoTracking().FirstOrDefaultAsync(x => x.No == txnNo);

                if (transactionNo is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Transaction no doesn't exist.");
                }

                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdraw = transactionNo
                };

                return Result<DepositWithdrawResponseModel>.Success(result, "Here are Transaction.");
            }
            catch (Exception ex)
            {
                return Result<DepositWithdrawResponseModel>.SystemError(ex.Message);
            }
        }

        public async Task<Result<DepositWithdrawResponseModel>> CreateDeposit(string mobileNo, decimal amount)
        {
            try
            {
                if (!mobileNo.StartsWith("09"))
                {
                    mobileNo = "09" + mobileNo;
                }

                var mobile = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);
                var LastId = await _db.TblDepositWithdraws.AsNoTracking().OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
                var currentId = LastId + 1;

                #region "Validation"
                if (mobileNo.Length > 11 || mobileNo.Length < 11)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Invalid mobile number length.");
                }
                if (mobile is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile number doesn't have wallet account.");
                }
                if (amount <= 0)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Please Enter valid amount.");
                }
                #endregion

                #region "Balance Calculation"
                mobile.Balance += amount;
                _db.TblWallets.Update(mobile);
                #endregion

                #region "deposit"
                //deposit.TransactionType = "CR";
                //deposit.TxnDate = DateTime.Now;
                //deposit.TxnNo = "DPT-" + deposit.TxnDate.ToString("yyyyMMdd") + "-" + currentId.ToString("D4"); //DPT-20241212-001

                var model = new TblDepositWithdraw
                {
                    Date = DateTime.Now,
                    No = "DPT-" + DateTime.Now.ToString("yyyyMMdd") + "-" + currentId.ToString("D4"), //DPT-20241212-001,
                    MobileNo = mobileNo,
                    Amount = amount,
                    TransactionType = "CR"
                };

                await _db.TblDepositWithdraws.AddAsync(model);
                await _db.SaveChangesAsync();
                #endregion

                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdraw = model
                };

                return Result<DepositWithdrawResponseModel>.Success(result, "Cash Deposit is successfully completed.");
            }
            catch (Exception ex)
            {
                return Result<DepositWithdrawResponseModel>.SystemError(ex.Message);
            }
        }

        public async Task<Result<DepositWithdrawResponseModel>> CreateWithdraw(string mobileNo, decimal amount)
        {
            try
            {
                if (!mobileNo.StartsWith("09"))
                {
                    mobileNo = "09" + mobileNo;
                }

                var mobile = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == mobileNo);
                var LastId = await _db.TblDepositWithdraws.AsNoTracking().OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
                var currentId = LastId + 1;

                #region "Validation"
                if (mobileNo.Length > 11 || mobileNo.Length < 11)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Invalid mobile number length.");
                }
                if (mobile is null)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("This mobile number doesn't have wallet account.");
                }
                if (amount <= 0)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Please Enter valid amount.");
                }
                if (mobile.Balance - amount < 10000)
                {
                    return Result<DepositWithdrawResponseModel>.ValidationError("Insufficient Balance.");
                }
                #endregion

                #region "Balance Calculation"
                mobile.Balance -= amount;
                _db.TblWallets.Update(mobile);
                #endregion

                #region "Withdraw"
                //withdraw.TransactionType = "DR";
                //withdraw.Date = DateTime.Now;
                //withdraw.No = "WTH-" + withdraw.Date.ToString("yyyyMMdd") + "-" + withdraw.Id.ToString("D4"); //DPT-20241212-001

                var model = new TblDepositWithdraw
                {
                    MobileNo = mobileNo,
                    Date = DateTime.Now,
                    No = "WTH-" + DateTime.Now.ToString("yyyyMMdd") + "-" + currentId.ToString("D4"), //DPT-20241212-001
                    Amount = amount,
                    TransactionType = "DR"
                };

                await _db.TblDepositWithdraws.AddAsync(model);
                await _db.SaveChangesAsync();
                #endregion

                var result = new DepositWithdrawResponseModel
                {
                    TblDepositWithdraw = model
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
