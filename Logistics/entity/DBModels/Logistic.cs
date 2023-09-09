using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Logistic
{
    public string LogisticsId { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Company { get; set; } = null!;

    public string ShipAddress { get; set; } = null!;

    public string PickAddress { get; set; } = null!;

    public virtual ICollection<Logisticsinfo> Logisticsinfos { get; set; } = new List<Logisticsinfo>();
}
