﻿using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Coupon
{
    public string CouponId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string StoreId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal? Discount { get; set; }

    public int? Bar { get; set; }

    public int? Reduction { get; set; }

    public DateTime Validfrom { get; set; }

    public DateTime Validto { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
