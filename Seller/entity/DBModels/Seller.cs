using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Seller
{
    public string UserId { get; set; } = null!;

    public string? SendAddress { get; set; }

    public virtual ICollection<SellerStore> SellerStores { get; set; } = new List<SellerStore>();

    public virtual User User { get; set; } = null!;
}
