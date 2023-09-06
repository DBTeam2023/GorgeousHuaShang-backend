namespace Payment.core.dto
{
    public class CouponAddDto
    {
        public string storeId { get; set; }
        public string commodityId { get; set; }
        public string type { get; set; }
        public decimal discount { get; set; }
        public int bar { get; set; }
        public int reduction { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

    }
}
