using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Store
{
    public string StoreId { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public decimal Score { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SellerStore> SellerStores { get; set; } = new List<SellerStore>();
}
