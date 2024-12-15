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
    public class TransactionService : ITransactionService
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
                model = Result<TransactionResponseModel>.NotFound("Sender not found.");
                goto Result;
            }

            if (receiver is null)
            {
                model = Result<TransactionResponseModel>.NotFound("Receiver not found.");
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
                model = Result<TransactionResponseModel>.ValidationError("Wrong Pin.");
                goto Result;
            }

            sender.Balance -= amount;
            receiver.Balance += amount;

            _db.TblWallets.Update(sender);
            _db.TblWallets.Update(receiver);

            var LastId = await _db.TblTransactions.AsNoTracking().OrderByDescending(x => x.TransactionId).Select(x => x.TransactionId).FirstOrDefaultAsync();
            var currentId = LastId + 1;

            var transaction = new TblTransaction
            {
                TransactionNo = "TRC-" + DateTime.Now.ToString("yyyyMMdd") + "-" + currentId.ToString("D4"),
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
                model = Result<TransactionResponseModel>.NotFound("Transaction not found.");
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
