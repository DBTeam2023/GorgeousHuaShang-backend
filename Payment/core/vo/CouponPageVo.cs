using Payment.utils;
using EntityFramework.Models;

namespace Payment.core.vo
{
    public class CouponPageVo
    {
        IPage<Coupon> Page;
        public CouponPageVo(List<Coupon> list,int current, int total, int size)
        {
            Page = IPage<Coupon>.builder()
                .records(list)
                .total(total)
                .size(size)
                .current(current)
                .build();
        }
    }
}
