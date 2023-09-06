using Product.dto;

namespace Product.domain.model.repository
{
    public interface CartRepository
    {
        //添加购物车表
        public Task add(string userId);

        //添加购物车商品
        public Task addItem(string userID,PickCountDto pick);

        public Task updateCart(CartAggregate cartAggregate);

      //更改款式
        public Task changePick(string userID, ChangePickDto changePickDto);

        //只改数量
       // public Task changePickCount(string userID, PickCountDto newpick);

        public Task deletePick(string userID,string pickID);

        //public CartAggregate getById(string userId);

        public Task<string> getUserId(string token);

    }
}
