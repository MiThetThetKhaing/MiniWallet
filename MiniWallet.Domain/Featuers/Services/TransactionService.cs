using Microsoft.EntityFrameworkCore;
using MiniWallet.Database.Models;
using MiniWallet.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Featuers.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _db;

        public TransactionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result<TransactionResponseModel>> CreateTransaction(string senderMobileNo, string receiverMobileNo, decimal amount, string notes, int Pin)
        {
            Result<TransactionResponseModel> model = new Result<TransactionResponseModel>();

            var sender = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == senderMobileNo);
            var receiver = await _db.TblWallets.AsNoTracking().FirstOrDefaultAsync(x => x.MobileNo == receiverMobileNo);

            if (sender is null)
            {
                model = Result<TransactionResponseModel>.ValidationError("Sender not found.");
                goto Result;
            }

            if (receiver is null)
            {
                model = Result<TransactionResponseModel>.ValidationError("Receiver not found.");
                goto Result;
            }

            if (sender.Balance < amount)
            {
                model = Result<TransactionResponseModel>.ValidationError("Insufficient balance.");
                goto Result;
            }

            if (string.IsNullOrWhiteSpace(notes))
            {
                model = Result<TransactionResponseModel>.ValidationError("You need to add some notes.");
                goto Result;
            }

            if (sender.PinCode != Pin)
            {
                model = Result<TransactionResponseModel>.ValidationError("Invalid Pin.");
                goto Result;
            }

            sender.Balance -= amount;
            receiver.Balance += amount;

            _db.TblWallets.Update(sender);
            _db.TblWallets.Update(receiver);

            var transaction = new TblTransaction
            {
                SenderMobileNo = senderMobileNo,
                ReceiverMobileNo = receiverMobileNo,
                Amount = amount,
                Notes = notes,
                TransactionDate = DateTime.UtcNow
            };

            await _db.TblTransactions.AddAsync(transaction);
            await _db.SaveChangesAsync();

            var response = new TransactionResponseModel
            {
                Transaction = transaction,
                SenderMobileNo = senderMobileNo,
                ReceiverMobileNo = receiverMobileNo
            };

            model = Result<TransactionResponseModel>.Success(response, "Transaction completed successfully.");

        Result:
            return model;
        }

        public async Task<Result<TransactionResponseModel>> GetTransaction(string transactionNo)
        {
            Result<TransactionResponseModel> model = new Result<TransactionResponseModel>();

            var transaction = await _db.TblTransactions.AsNoTracking().FirstOrDefaultAsync(x => x.TransactionNo == transactionNo);

            if (transaction is null)
            {
                model = Result<TransactionResponseModel>.ValidationError("Transaction not found.");
                goto Result;
            }

            var response = new TransactionResponseModel
            {
                Transaction = transaction,
                SenderMobileNo = transaction.SenderMobileNo,
                ReceiverMobileNo = transaction.ReceiverMobileNo
            };
            model = Result<TransactionResponseModel>.Success(response, "Transaction retrieved successfully.");

        Result:
            return model;
        }
    }
}
