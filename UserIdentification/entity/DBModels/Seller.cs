using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Seller
{
    public string UserId { get; set; } = null!;

    public string SendAddress { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
