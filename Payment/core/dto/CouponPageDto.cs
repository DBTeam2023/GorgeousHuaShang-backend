namespace Payment.core.dto
{
    public class CouponPageDto
    {
        public int PageNo { get; set; }
        public int pageSize { get; set; }

        public string? userId { get; set; } = null!;

        public string? storeId { get; set; } = null!;
        public string? commodityId { get; set; } = null!;

        public string? storeName { get; set; } = null!;
        public string? commodityName { get; set; } = null!;

    }
}
