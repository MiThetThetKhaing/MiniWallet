using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class TransferRequestModel
    {
        public string senderMobileNo { get; set; }
        public string receiverMobileNo { get; set; }
        public decimal amount { get; set; }
        public string notes { get; set; }
        public int Pin { get; set; }
    }
}
