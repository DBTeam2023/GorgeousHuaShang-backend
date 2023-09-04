using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Order.common;
using Order.domain.model;
using Order.domain.model.repository;
using Order.domain.service;
using Order.dto;
using Order.utils;

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

        public async Task<PickInfoDto[]> getPickInfo(string[] pickID)
        {
            var pickInfoDto = await _orderService.getPickInfos(pickID);
            return pickInfoDto;
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
            // 查询名称，电话号码和地址
            var buyerInfo = _orderService.getBuyerInfo(order.UserID);
            var orderAggregate = OrderAggregate.Create(order, buyerInfo);
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
