using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public DateTime? LoginTime { get; set; }

    public string Password { get; set; } = null!;

    public string? NickName { get; set; }

    public string Type { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual Seller? Seller { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
