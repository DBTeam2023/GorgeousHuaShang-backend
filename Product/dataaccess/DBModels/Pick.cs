using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Pick
{
    public string CommodityId { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string PickId { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public string? PropertyValue { get; set; }

    public decimal Stock { get; set; }

    public virtual CommodityProperty? CommodityProperty { get; set; }
}
