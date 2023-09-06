namespace Payment.core.dto
{
    public class MyCouponDto
    {
        public int PageNo { get; set; }
        public int pageSize { get; set; }

        public string? storeId { get; set; } = null!;
        public string? storeName { get; set; } = null!;
    }
}
