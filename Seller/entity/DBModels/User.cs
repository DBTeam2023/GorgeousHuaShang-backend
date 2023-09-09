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

    public virtual Buyer? Buyer { get; set; }

    public virtual Seller? Seller { get; set; }
}
