using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Category
{
    public string CommodotyId { get; set; } = null!;

    public string? Type { get; set; }
}
