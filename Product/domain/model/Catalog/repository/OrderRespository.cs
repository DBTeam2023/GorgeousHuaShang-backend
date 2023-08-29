using Product.dto;
using Product.utils;

namespace Product.domain.model.repository
{
    public interface OrderRepository
    {
        public Task add(OrderAggregate productAggregate);

        public Task update(OrderAggregate productAggregate);

        public OrderAggregate getById(string OrderId);

        public Task delete(string OrderId);
    }
}
