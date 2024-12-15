using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWallet.Domain.Models
{
    public class ChangePin
    {
        public int oldPin { get; set; }
        public int newPin { get; set; }
    }
}
