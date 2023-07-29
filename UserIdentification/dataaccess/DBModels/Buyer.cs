using System;
using System.Collections.Generic;

namespace UserIdentification.dataaccess.DBModels;

public partial class Buyer
{
    public string UserId { get; set; } = null!;

    public string? ReceiveAddress { get; set; }

    public byte? Age { get; set; }

    public bool? Gender { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public bool IsVip { get; set; }

    public virtual User User { get; set; } = null!;
}
