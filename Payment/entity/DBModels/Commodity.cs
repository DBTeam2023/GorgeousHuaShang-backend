using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Commodity
{
    public string CommodityId { get; set; } = null!;

    public string CommodityName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public string StoreId { get; set; } = null!;

    public int? StockQuantity { get; set; }

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<Coupontemp> Coupontemps { get; set; } = new List<Coupontemp>();

    public virtual Store Store { get; set; } = null!;
}
