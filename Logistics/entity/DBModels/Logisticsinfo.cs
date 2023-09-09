using System;
using System.Collections.Generic;

namespace EntityFramework.Models;

public partial class Logisticsinfo
{
    public string LogisticsId { get; set; } = null!;

    public string ArrivePlace { get; set; } = null!;

    public DateTime ArriveTime { get; set; }

    public virtual Logistic Logistics { get; set; } = null!;
}


