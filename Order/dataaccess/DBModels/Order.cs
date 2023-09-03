using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Order
{
    public string OrderId { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public decimal Money { get; set; }

    public bool State { get; set; }

    public bool IsDeleted { get; set; }

    public string LogisticsId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual ICollection<OrderPick> OrderPicks { get; set; } = new List<OrderPick>();

    public virtual User User { get; set; } = null!;
}
