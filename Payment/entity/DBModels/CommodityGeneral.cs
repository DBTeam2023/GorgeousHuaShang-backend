using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class CommodityGeneral
{
    public string CommodityId { get; set; } = null!;

    public string CommodityName { get; set; } = null!;

    public string? Description { get; set; }

    public string StoreId { get; set; } = null!;

    public string Price { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual Store Store { get; set; } = null!;
}
