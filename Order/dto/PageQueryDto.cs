using Order.exception;

namespace Order.dto
{
    public class PageQueryDto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        // 订单查询条件
        public string? OrderId { get; set; }

        public decimal? Moneymin { get; set; }

        public decimal? Moneymax { get; set; }

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
