using EntityFramework.Models;
namespace Payment.core.vo
{
    public class CouponVo
    {
        public string? userId { get; set; }
        public string couponId { get; set; }
        public string storeId { get; set; }
        public string type { get; set; }
        public decimal? discount { get; set; }
        public decimal? bar { get; set; }
        public decimal? reduction { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public CouponVo(Coupon x)
        {
            userId = x.UserId;
            couponId = x.CouponId;
            storeId = x.StoreId;    
            type = x.Type;
            discount = x.Discount;
            bar = x.Bar;
            reduction = x.Reduction;
            start = x.Validfrom;
            end = x.Validto;
            
        }
    }
}
