using Order.domain.model;
using Order.dto;
using Order.utils;

namespace Product.application
{
    public interface OrderApplicationService
    {
        // 可以不要
        public Task UpdateOrder(OrderDto order);
        public OrderAggregate getOrder(string orderID);
        // TODO 分页查询
        public OrderAggregate[] getAllOrder(string ID);
        public Task DeleteOrder(string orderID);
        // 创建

        //分页查询
        public IPage<OrderAggregate> orderPageQuery(PageQueryDto pageQuery);
    }
}
