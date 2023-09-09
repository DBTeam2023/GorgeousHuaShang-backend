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
            var x = await couponService.GenerateCoupon(couponAdd.storeId,couponAdd.type, couponAdd.discount, couponAdd.bar, couponAdd.reduction, couponAdd.start, couponAdd.end);
            return ComResponse<CouponTempVo>.success(new CouponTempVo(x));
        }

        [HttpPost]
        public async Task<ComResponse<CouponVo>> add([FromBody] CouponUserDto couponUser)
        {
            var x = await couponService.AddCoupon(couponUser.userId, couponUser.couponId);
            return ComResponse<CouponVo>.success(new CouponVo(x));
        }

        [HttpPost]
        public async Task deleteByUser([FromBody] CouponDelDto couponDel)
        {
            string token = Request.Headers["Authorization"].ToString();
            await couponService.DelCouponByBuyer(couponDel.couponId, token);
        }

        [HttpPost]
        public async Task deleteByStore([FromBody] CouponDelDto couponDel)
        {
            await couponService.DelCouponByStore(couponDel.couponId);
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

            IPage<Coupon> x = await couponService.getPage(couponPage.PageNo, couponPage.pageSize, couponPage.userId, couponPage.storeId, couponPage.storeName);
            return ComResponse<IPage<Coupon>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Coupontemp>>> getStoreCoupon([FromBody] CouponTempDto couponTempDto)
        {
            IPage<Coupontemp> x = await couponService.getStoreCoupon(couponTempDto.pageNo, couponTempDto.pageSize, couponTempDto.storeId);
            return ComResponse<IPage<Coupontemp>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Coupon>>> getMyCoupon([FromBody] MyCouponDto myCouponDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await couponService.getUserId(token);
            IPage<Coupon> x = await couponService.getPage(myCouponDto.PageNo, myCouponDto.pageSize, userId, myCouponDto.storeId, myCouponDto.storeName);
            return ComResponse<IPage<Coupon>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<IPage<Coupon>>> getValid([FromBody] GetValidDto getValidDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            IPage<Coupon> x = await couponService.getValid(getValidDto.pageNo, getValidDto.pageSize, token, getValidDto.pickIds);
            return ComResponse<IPage<Coupon>>.success(x);
        }

        [HttpPost]
        public async Task<ComResponse<decimal?>> calculate([FromBody] CalDto calDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            var x = await couponService.calculate(calDto.pickIds, calDto.couponId, token);
            return ComResponse<decimal?>.success(x);
        }
    }
}
