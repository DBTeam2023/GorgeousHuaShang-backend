using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Category
{
    public string CommodityId { get; set; } = null!;

    public string? Type { get; set; }

    public virtual CommodityGeneral Commodity { get; set; } = null!;
}
