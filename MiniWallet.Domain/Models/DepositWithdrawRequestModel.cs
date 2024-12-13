using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class DepositWithdrawRequestModel
    {
        public int Id { get; set; }
        public string TxnNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime TxnDate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }
}
