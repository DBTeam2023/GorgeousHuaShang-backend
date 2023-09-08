using Order.domain.model;
using Order.dto;
using Order.utils;

namespace Order.application
{
    public interface OrderApplicationService
    {
        public Task ChangeOrderCancel(string token, string orderID);
        public Task ChangeOrderPaidComplete(string token, string orderID);
        public Task ChangeOrderPaidSuccess(string token, string orderID);
        public Task<OrderAggregate> getOrderInfo(string token, string orderID);
        public Task DeleteOrder(string orderID);
        public Task<OrderAggregate> createOrder(string token, CreateOrderDto order);
        //分页查询
        public Task<IPage<OrderAggregate>> orderPageQuery(string token,PageQueryDto pageQuery);
    }
}
