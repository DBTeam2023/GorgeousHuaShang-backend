using Order.utils;
using Order.dto;

namespace Order.domain.model.repository
{
    public interface OrderRepository
    {
        public Task add(OrderAggregate orderAggregate);

        public Task update(OrderAggregate orderAggregate);

        public OrderAggregate getById(string OrderId);

        public Task delete(string OrderId);

        public IPage<OrderAggregate> pageQuery(PageQueryDto pageQuery);
    }
}
