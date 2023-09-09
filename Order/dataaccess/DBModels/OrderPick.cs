using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class OrderPick
{
    public string OrderId { get; set; } = null!;

    public string PickId { get; set; } = null!;

    public decimal Number { get; set; }

    public virtual Myorder Order { get; set; } = null!;
}
