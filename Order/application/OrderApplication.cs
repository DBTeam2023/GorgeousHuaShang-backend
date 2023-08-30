using Product.domain.model;
using Product.dto;
using Product.utils;

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

    }
}
