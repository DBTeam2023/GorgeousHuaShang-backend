using System;
using System.Collections.Generic;

namespace EntityFramework.Models;


public partial class Order
{
    public string ID { get; set; } = null!;
    public string Time { get; set; } = null!;
    public float Money { get; set; }
    public int State { get; set; }
    public bool IsDeleted { get; set; }
    public string LogisticsID { get; set; } = null!;
    public string StoreID { get; set; } = null!;
    public string UserID { get; set; } = null!;
}
