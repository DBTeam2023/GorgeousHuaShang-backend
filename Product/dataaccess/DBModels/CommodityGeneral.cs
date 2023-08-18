using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class CommodityGeneral
{
    public string CommodityId { get; set; } = null!;

    public string CommodityName { get; set; } = null!;

    public string? Description { get; set; }

    public string StoreId { get; set; } = null!;

    public decimal Price { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = new List<CommodityProperty>();
}
