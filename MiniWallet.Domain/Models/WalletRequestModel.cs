using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class WalletRequestModel
    {
        public string WalletUserName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string MobileNo { get; set; } = null!;

        public decimal Balance { get; set; }

        public int PinCode { get; set; }
    }
}
