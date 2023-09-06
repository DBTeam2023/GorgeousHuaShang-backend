using Product.domain.model;
using Product.dto;
using Product.utils;


namespace Product.application
{
    public interface CartApplicationService
    {
        //商品详情页，所以加入购物车的时候还要commodityid吗？不需要也可以，因为有了pick我就可以从数据库里获取到他对应的commodity,只需要看有没有添加成功就可以，不需要返回
        public Task addCartItem(string userId,PickCountDto pick);
        //购物车
        //单选删除
        public Task deleteCartItem(string userId, PickIdDto pickId);

        //批量删除
        public Task deleteCartItem(string userId, List<PickIdDto> picks);

        //更新pick，还要看原有购物车里面有咩有这个pick，如果有的话就在原有的基础上+1，这里的更新包含所有的更新，一种情况是修改pick，一种情况是修改数量
        //public Task updateCartItem(string userId, PickIdDto oldPickId,PickCountDto newPick);


        //用于前端展示购物车列表，还要把所有属性传给他,获取商品所有信息
        //public Task<CartAggregate> displayCartItems(UserIdDto userId);



    


    }
}
