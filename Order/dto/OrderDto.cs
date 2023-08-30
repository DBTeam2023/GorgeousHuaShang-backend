namespace Product.dto
{
    public class OrderDto
    {
        public string OrderId { get; set; } = null!;

        public string CreateTime { get; set; } = null!;

        public decimal Money { get; set; }

        public int State { get; set; }

        public string LogisticsID { get; set; } = null!;
        public string StoreID { get; set; } = null!;
        public string UserID { get; set; } = null!;

        public bool? IsDeleted { get; set; } = false;

    }
}
