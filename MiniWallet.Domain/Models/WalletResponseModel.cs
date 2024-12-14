using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniWallet.Database.Models;

namespace MiniWallet.Domain.Models
{
    public class WalletResponseModel
    {
        public TblWallet Wallet { get; set; } 
    }
}
