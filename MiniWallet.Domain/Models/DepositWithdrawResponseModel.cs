using MiniWallet.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class DepositWithdrawResponseModel
    {
        public TblDepositWithdraw TblDepositWithdraw { get; set; }
    }
}
