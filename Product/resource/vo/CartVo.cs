using Product.dto;

namespace Product.resource.vo
{
    public class CartVo
    {
        public List<CartItemSingleVo> Items { get; set; }

        public CartVo(List<IGrouping<string, CartItemSingleVo>> cartItems)
        {
            Items = new List<CartItemSingleVo>();
            foreach (var group in cartItems)
            {
                foreach (var cartItem in group)
                {
                    Items.Add(cartItem);
                }
            }
        }
    }
}
