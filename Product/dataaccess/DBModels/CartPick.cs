using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class CartPick
{
    public string UserId { get; set; } = null!;

    public decimal PickCount { get; set; }

    public string PickId { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual Cart User { get; set; } = null!;
}
