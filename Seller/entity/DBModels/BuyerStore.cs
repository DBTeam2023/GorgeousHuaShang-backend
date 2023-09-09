using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class BuyerStore
{
    public string UserId { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public byte? Hasbought { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual Buyer User { get; set; } = null!;
}
