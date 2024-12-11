using System;
using System.Collections.Generic;

namespace MiniWallet.Database.Models;

public partial class TblWallet
{
    public int WalletUserId { get; set; }

    public string WalletUserName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public int PinCode { get; set; }

    public decimal Balance { get; set; }
}
