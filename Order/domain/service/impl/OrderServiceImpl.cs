using EntityFramework.Context;
using Order.domain.model;
using Order.domain.model.repository;
using Order.dto;
using Order.utils;

namespace Order.domain.service.impl
{
    public class OrderServiceImpl : OrderService
    {
        private readonly ModelContext modelContext;
        public OrderRepository orderRepository;
        public OrderServiceImpl(ModelContext _modelContext, OrderRepository _orderRepository)
        {
            modelContext = _modelContext;
            orderRepository = _orderRepository;
        }


      

    }
}
