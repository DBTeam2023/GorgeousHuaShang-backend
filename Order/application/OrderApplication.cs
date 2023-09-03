using Order.domain.model;
using Order.dto;
using Order.utils;

namespace Order.application
{
    public interface OrderApplicationService
    {
        public Task UpdateOrder(OrderDto order);
        public OrderAggregate getOrderInfo(string orderID);
        public Task DeleteOrder(string orderID);
        // 创建
        public Task<OrderIdDto> createOrder(CreateOrderDto order);
        //分页查询
        public IPage<OrderAggregate> orderPageQuery(PageQueryDto pageQuery);
    }
}
