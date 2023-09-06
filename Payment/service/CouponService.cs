using EntityFramework.Models;
using Payment.utils;
namespace Payment.service
{
    public interface CouponService
    {
        public Task<Coupontemp> GenerateCoupon(string storeId, string type, decimal discount, int bar, int reduction, DateTime start, DateTime end);
        public Task<Coupon> AddCoupon(string userId, string couponId);
        public Task DelCouponByBuyer(string couponId, string token);
        public Task DelCouponByStore(string couponId);
        public Task<Coupon> getInfo(string couponId);
        public Task<IPage<Coupon>> getPage(int PageNo, int PageSize, string? userId, string? storeId, string? storeName);
        public Task Clean();
        public Task<IPage<Coupontemp>> getStoreCoupon(int pageNo, int pageSize, string storeId);
        public Task<IPage<Coupon>> getValid(int pageNo, int pageSize, string token, List<string> pickIds);
        public Task<decimal?> calculate(List<string> pickIds, string couponId, string token);
        public Task<string> getUserId(string token);
    }
}
