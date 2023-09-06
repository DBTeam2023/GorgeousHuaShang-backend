using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class SellerStore
{
    public string UserId { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public decimal Ismanager { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual Seller User { get; set; } = null!;
}
