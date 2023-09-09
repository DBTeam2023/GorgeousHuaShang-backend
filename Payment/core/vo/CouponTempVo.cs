using EntityFramework.Models;
namespace Payment.core.vo
{
    public class CouponTempVo
    {
        public string couponId { get; set; }
        public string storeId { get; set; }
        public string type { get; set; }
        public decimal? discount { get; set; }
        public decimal? bar { get; set; }
        public decimal? reduction { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public CouponTempVo(Coupontemp x)
        {
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
