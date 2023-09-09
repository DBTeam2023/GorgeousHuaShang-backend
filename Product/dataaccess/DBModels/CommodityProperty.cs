using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class CommodityProperty
{
    public string CommodityId { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public string PropertyValue { get; set; } = null!;

    public virtual CommodityGeneral Commodity { get; set; } = null!;

    public virtual ICollection<Pick> Picks { get; set; } = new List<Pick>();
}
