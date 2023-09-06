using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Product.application;
using Product.domain.model.repository;
using Product.domain.service;
using Product.dto;
using Product.exception;
using Product.resource.vo;
using Product.utils;

namespace Product.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CartController :ControllerBase
    {
        private CartApplicationService cartApplicationService;
        private CartRepository cartRepository;
        private CartService cartService;
        public CartController(CartApplicationService _cartApplicationService, CartRepository _cartRepository, CartService _cartService)
        {
            cartApplicationService = _cartApplicationService;
            cartRepository = _cartRepository;
            cartService = _cartService;
        }

        //获取购物车列表的时候建表，如果就已经有了就拉取，没有就新建一个表
        [HttpPost]
        public async Task<ComResponse<CartVo>> getCart()
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await cartRepository.getUserId(token);
            //var ans = cartService.displayCartItems(userId);
            //return ComResponse<CartVo>.success(new CartVo(ans));
           
               var ans = cartService.displayCartItems(userId);
                return ComResponse<CartVo>.success(new CartVo(ans));
           
            

        }

        //添加商品
        [HttpPost]
        public async Task<ComResponse<string>> addItem([FromBody] PickCountDto pickCount)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await cartRepository.getUserId(token);
            await cartApplicationService.addCartItem(userId, pickCount);
            return ComResponse<string>.success("成功加入");
        }

        //删除商品
        //[HttpPost]
        //public async Task<ComResponse<string>> deleteItem([FromBody]PickIdDto pickId)
        //{
        //    string token = Request.Headers["Authorization"].ToString();
        //    string userId = await cartRepository.getUserId(token);

        //    await cartApplicationService.deleteCartItem(userId, pickId);
        //    return ComResponse<string>.success("成功删除");
        //}

        [HttpPost]
        public async Task<ComResponse<string>>deleteItems([FromBody] List<PickIdDto> picks)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await cartRepository.getUserId(token);

            await cartApplicationService.deleteCartItem(userId, picks);
            return ComResponse<string>.success("成功删除");
        }

        //更新item
        [HttpPost]
        public async Task<ComResponse<CartItemSingleVo>> changeItem([FromBody] ChangePickDto changePickDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            string userId = await cartRepository.getUserId(token);
            var ans = await cartService.changeItem(userId, changePickDto);
            return ComResponse<CartItemSingleVo>.success(ans);
        }


    }
}
