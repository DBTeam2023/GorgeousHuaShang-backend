﻿using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Order.common;
using Order.domain.model;
using Order.domain.model.repository;
using Order.domain.service;
using Order.dto;
using Order.resource.remote;
using Order.utils;
using Product.utils;

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

       
        public async Task<OrderAggregate> getOrderInfo(string token,string orderID)
        {
            return await _orderRepository.getById(token,orderID);
        }

        public async Task ChangeOrderCancel(string token, string orderID)
        {
            var logisticsId=await _orderService.ChangeOrderCancel(token, orderID);
        }

       

        public async Task ChangeOrderPaidSuccess(string token, string orderID)
        {
            await _orderService.ChangeOrderPaidSuccess(token, orderID);
        }

        public async Task ChangeOrderPaidComplete(string token, string orderID)
        {
            await _orderService.ChangeOrderPaidComplete(token, orderID);
        }


        //ok
        public async Task DeleteOrder(string orderID)
        {
            await _orderRepository.delete(orderID);
            

        }
        // ok
        public async Task<OrderAggregate> createOrder(string token,CreateOrderDto order)
        {
            var buyerInfo = await UserRemote.getUserInfo(token);
            var orderInfo = await PickRemote.getPickInfos(order);

            var orderAggregate = await OrderAggregate.Create(buyerInfo,orderInfo);
            await _orderRepository.add(orderAggregate);

            return orderAggregate;
        }

        //ok
        public async Task<IPage<OrderAggregate>> orderPageQuery(string token,PageQueryDto pageQuery)
        {
            return await _orderRepository.pageQuery(token,pageQuery);
        }

    }
}
