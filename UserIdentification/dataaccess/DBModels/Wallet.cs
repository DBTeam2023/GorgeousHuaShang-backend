using System;
using System.Collections.Generic;

namespace UserIdentification.dataaccess.DBModels;

public partial class Wallet
{
    public string UserId { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool Status { get; set; }

    public virtual User User { get; set; } = null!;
}
