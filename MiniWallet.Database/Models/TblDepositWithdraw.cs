using System;
using System.Collections.Generic;

namespace MiniWallet.Database.Models;

public partial class TblDepositWithdraw
{
    public int Id { get; set; }

    public string No { get; set; } = null!;

    public DateTime Date { get; set; }

    public string MobileNo { get; set; } = null!;

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;
}
