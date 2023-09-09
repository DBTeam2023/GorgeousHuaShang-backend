using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Store
{
    public string StoreId { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public decimal Score { get; set; }

    public bool IsDeleted { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<BuyerStore> BuyerStores { get; set; } = new List<BuyerStore>();

    public virtual ICollection<SellerStore> SellerStores { get; set; } = new List<SellerStore>();
}
