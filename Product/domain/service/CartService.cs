using Product.domain.model;
using Product.dto;
using Product.resource.vo;

namespace Product.domain.service
{
    public interface CartService
    {
        //public ProductService productService;
        public List<IGrouping<string, CartItemSingleVo>> displayCartItems(string userId);

        public Task<CartItemSingleVo> changeItem(string userID, ChangePickDto changePickDto);
    }
}
