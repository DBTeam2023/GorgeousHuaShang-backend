using System;
using System.Collections.Generic;

namespace UserIdentification.dataaccess.DBModels;

public partial class Seller
{
    public string UserId { get; set; } = null!;

    public string? SendAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
