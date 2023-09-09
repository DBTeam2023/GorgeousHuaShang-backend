using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Cart
{
    public decimal TotalQuantity { get; set; }

    public decimal TotalAmount { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<CartPick> CartPicks { get; set; } = new List<CartPick>();

    public virtual User User { get; set; } = null!;
}
