using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Order.common;
using Order.domain.model;
using Order.domain.model.repository;
using Order.domain.service;
using Order.dto;
using Order.utils;
using Product.domain.model;
using Product.domain.model.repository;
using Product.domain.service;
using Product.dto;
using Product.resource.vo;

namespace Order.application.impl
{
    public class OrderApplicationServiceImpl : OrderApplicationService
    {

        private OrderRepository _orderRepository; 
        private OrderService _orderService;

        public OrderApplicationServiceImpl(OrderRepository orderRepository, OrderService orderService)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
        }


        public async Task UpdateOrder(OrderDto order)
        {
            var orderAggregate = OrderAggregate.Create(order);
            await _orderRepository.update(orderAggregate);
        }
        public OrderAggregate getOrderInfo(string orderID)
        {
            return _orderRepository.getById(orderID);
        }
        public async Task DeleteOrder(string orderID)
        {
            await _orderRepository.delete(orderID);

        }
        // 创建
        public async Task<OrderIdDto> createOrder(CreateOrderDto order)
        {
            var orderAggregate = OrderAggregate.Create(order);
            await _orderRepository.add(orderAggregate);
            return new OrderIdDto { OrderId = orderAggregate.OrderID };
        }

        //分页
        public IPage<OrderAggregate> orderPageQuery(PageQueryDto pageQuery)
        {
            return _orderRepository.pageQuery(pageQuery);
        }

    }
}
