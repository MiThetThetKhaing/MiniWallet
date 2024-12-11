using System;
using System.Collections.Generic;

namespace MiniWallet.Database.Models;

public partial class TblTransaction
{
    public int TransactionId { get; set; }

    public string TransactionNo { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public string SenderMobileNo { get; set; } = null!;

    public string ReceiverMobileNo { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Notes { get; set; } = null!;
}
