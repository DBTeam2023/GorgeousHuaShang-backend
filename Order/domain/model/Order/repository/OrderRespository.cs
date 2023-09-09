using Order.utils;
using Order.dto;

namespace Order.domain.model.repository
{
    public interface OrderRepository
    {
        public Task add(OrderAggregate orderAggregate);

        public Task update(OrderAggregate orderAggregate);

        public Task<OrderAggregate> getById(string token,string OrderId);

        public Task delete(string OrderId);

        public Task<IPage<OrderAggregate>> pageQuery(string token,PageQueryDto pageQuery);
    }
}
