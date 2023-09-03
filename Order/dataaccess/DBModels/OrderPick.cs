using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class OrderPick
{
    public string OrderId { get; set; } = null!;

    public string PickId { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
