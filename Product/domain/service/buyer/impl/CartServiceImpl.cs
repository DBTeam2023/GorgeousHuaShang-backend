using EntityFramework.Context;
using Product.domain.model;
using Product.domain.service;
using Product.dto;
using Product.resource.vo;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Product.domain.model.repository;
using Microsoft.AspNetCore.Mvc;
using Product.exception;

namespace Product.domain.service.impl
{
    public class CartServiceImpl :CartService
    {
        private readonly ModelContext _context;
        // CategoryRepository _categoryRepository;
        private ProductService _productService;

        private CartRepository _cartRepository;
        public CartServiceImpl(ModelContext context, ProductService productService, CartRepository cartRepository)
        {
            _context = context;
            _productService = productService;
            _cartRepository = cartRepository;
        }
     
        //按照commodityid划分传给前端,还要看是否下架。有失效商品显示
        //按照commodityid分组
        public List<IGrouping<string, CartItemSingleVo>> displayCartItems(string userId)
        {
            var db_cart = _context.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            if (db_cart == null)
            {
                _cartRepository.add(userId);
            }
            var db_cartItems = _context.CartPicks.Where(c => c.UserId == userId).ToList();

            if (db_cart.TotalQuantity == 0)
                throw new NullException("购物车为空");
            var ans = new List<CartItemSingleVo>();
            foreach(var item in db_cartItems)
            {
                var db_pick = _context.Picks.Where(c => c.PickId == item.PickId).FirstOrDefault();
                var db_commodity = _context.CommodityGenerals.Where(c => c.CommodityId == db_pick.CommodityId).FirstOrDefault();
                var db_ori_property = _context.CommodityProperties.Where(c => c.CommodityId == db_pick.CommodityId).ToList();
                var property_list = transferToDModelProperty(db_ori_property);
                var pick_properties = _productService.displayPicks(new CommodityIdDto { commodityId = db_pick.CommodityId });
                
                ans.Add(new CartItemSingleVo(new CartItemDto(db_pick.CommodityId, db_commodity.CommodityName, db_pick.IsDeleted, db_pick.Price, db_pick.Description, db_pick.PickId, property_list, pick_properties, db_pick.Stock, item.PickCount)));

                
            }
           var groupedCartItems = ans.GroupBy(c => c.ProductId).ToList();


            return groupedCartItems;
        }
        internal Dictionary<string, List<string>> transferToDModelProperty(List<CommodityProperty> property)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            foreach (var item in property)
            {
                string key = item.PropertyType; // 使用第一个字母作为键
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key].Add(item.PropertyValue); // 如果键已存在，将值添加到对应的列表中
                }
                else
                {
                    dictionary[key] = new List<string> { item.PropertyValue }; // 如果键不存在，创建一个新的列表并添加值
                }
            }
            return dictionary;

        }
        public async Task<CartItemSingleVo> changeItem(string userID, ChangePickDto changePickDto)
        {
            await _cartRepository.changePick(userID, changePickDto);
           
            var db_pick = _context.Picks.Where(c => c.PickId == changePickDto.newPickId).FirstOrDefault();
            var db_commodity = _context.CommodityGenerals.Where(c => c.CommodityId == db_pick.CommodityId).FirstOrDefault();
            var db_ori_property = _context.CommodityProperties.Where(c => c.CommodityId == db_pick.CommodityId).ToList();
            var property_list = transferToDModelProperty(db_ori_property);
            var pick_properties = _productService.displayPicks(new CommodityIdDto { commodityId = db_pick.CommodityId });
            
            return new CartItemSingleVo(new CartItemDto(db_pick.CommodityId, db_commodity.CommodityName, db_pick.IsDeleted, db_pick.Price, db_pick.Description, db_pick.PickId, property_list, pick_properties, db_pick.Stock, changePickDto.count));

        }
    }
}
