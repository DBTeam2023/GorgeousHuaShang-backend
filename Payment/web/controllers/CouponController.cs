using Microsoft.AspNetCore.Mvc;
using Payment.core.dto;
using Payment.core.vo;
using Payment.service;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Payment.utils;

namespace Payment.web.controllers
{
    [ApiController]
    [Route("Payment/[controller]/[action]")]
    public class CouponController: ControllerBase
    {
        public static CouponService couponService;
        public CouponController(CouponService _couponService)
        {
            couponService = _couponService;
        }

        [HttpPost]
        public async Task<ComResponse<CouponTempVo>> generate([FromBody] CouponAddDto couponAdd)
        {
            var x = await couponService.GenerateCoupon(couponAdd.storeId, couponAdd.commodityId, couponAdd.type, couponAdd.discount, couponAdd.bar, couponAdd.reduction, couponAdd.start, couponAdd.end);
            return ComResponse<CouponTempVo>.success(new CouponTempVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<CouponVo>> add([FromBody] CouponUserDto couponUser)
        {
            var x = await couponService.AddCoupon(couponUser.userId, couponUser.couponId);
            return ComResponse<CouponVo>.success(new CouponVo(x));
        }

        [HttpDelete]
        public async Task delete([FromBody] CouponDelDto couponDel)
        {
            await couponService.DelCoupon(couponDel.couponId);
        }

        [HttpPost]
        public async Task<ComResponse<CouponVo>> getInfo([FromBody] CouponInfoDto couponInfo)
        {
            var x = await couponService.getInfo(couponInfo.couponId);
            return ComResponse<CouponVo>.success(new CouponVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Coupon>>> getPage([FromBody] CouponPageDto couponPage)
        {
            List<Coupon> x = await couponService.getPage(couponPage.PageNo, couponPage.pageSize, couponPage.userId, couponPage.storeId, couponPage.commodityId);
            var list = x.Skip((couponPage.PageNo - 1) * couponPage.pageSize)
               .Take(couponPage.pageSize)
               .ToList();
            IPage<Coupon> Page = IPage<Coupon>.builder()
               .records(list)
               .total(x.Count)
               .size(couponPage.pageSize)
               .current(couponPage.PageNo)
               .build();
            return ComResponse<IPage<Coupon>>.success(Page);
        }
    }
}
