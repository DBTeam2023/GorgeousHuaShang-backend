using EntityFramework.Models;
namespace Payment.service
{
    public interface CouponService
    {
        public Task<Coupontemp> GenerateCoupon(string storeId, string commodityId, string type, decimal discount, int bar, int reduction, DateTime start, DateTime end);
        public Task<Coupon> AddCoupon(string userId, string couponId);
        public Task DelCoupon(string couponId);

        public Task<Coupon> getInfo(string couponId);
        public Task<List<Coupon>> getPage(int current, int size, string? userId, string? storeId, string? commodityId);

        public Task Clean();
      
    }
}
