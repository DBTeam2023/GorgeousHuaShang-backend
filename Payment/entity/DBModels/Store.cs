using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Store
{
    public string StoreId { get; set; } = null!;

    public string StoreName { get; set; } = null!;

    public decimal Score { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Commodity> Commodities { get; set; } = new List<Commodity>();

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<Coupontemp> Coupontemps { get; set; } = new List<Coupontemp>();
}
