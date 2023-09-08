using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Myorder
{
    public string OrderId { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public decimal Money { get; set; }

    public string State { get; set; } = null!;

    public string LogisticsId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual ICollection<OrderPick> OrderPicks { get; set; } = new List<OrderPick>();
}
