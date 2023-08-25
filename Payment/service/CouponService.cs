using EntityFramework.Models;
using Payment.utils;
namespace Payment.service
{
    public interface CouponService
    {
        public Task<Coupontemp> GenerateCoupon(string storeId, string commodityId, string type, decimal discount, int bar, int reduction, DateTime start, DateTime end);
        public Task<Coupon> AddCoupon(string userId, string couponId);
        public Task DelCoupon(string couponId);

        public Task<Coupon> getInfo(string couponId);
        public Task<IPage<Coupon>> getPage(int PageNo, int PageSize, string? userId, string? storeId, string? commodityId,  string? storeName, string? commodityName);

        public Task Clean();
      
    }
}
