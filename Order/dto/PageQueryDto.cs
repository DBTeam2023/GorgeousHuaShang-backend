using Order.exception;

namespace Order.dto
{
    public class OrderPageQueryDto
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

        // 订单查询条件
        public int? OrderId { get; set; }

        public string? UserID { get; set; }

        public decimal? Moneymin { get; set; }

        public decimal? Moneymax { get; set; }

        public string? CommodityId { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? OrderStatus { get; set; }

        public void Check()
        {
            if (PageSize <= 0)
                throw new PageException("Page size should be positive");
            if (PageIndex <= 0)
                throw new PageException("Page index should be larger than 0");
        }
    }
}
