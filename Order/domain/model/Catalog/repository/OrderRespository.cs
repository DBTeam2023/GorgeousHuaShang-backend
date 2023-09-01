using Product.dto;
using Product.utils;

namespace Product.domain.model.repository
{
    public interface OrderRepository
    {
        public Task add(OrderAggregate orderAggregate);

        public Task update(OrderAggregate orderAggregate);

        public OrderAggregate getById(string OrderId);

        public Task delete(string OrderId);
    }
}
