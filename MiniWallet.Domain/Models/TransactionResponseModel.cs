using MiniWallet.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class TransactionResponseModel
    {
        public TblTransaction Transaction { get; set; }
        public string SenderMobileNo { get; set; }
        public string ReceiverMobileNo { get; set; }
    }
}
