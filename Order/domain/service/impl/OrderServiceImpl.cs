using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Order.domain.model;
using Order.domain.model.repository;
using Order.dto;
using Order.exception;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using Order.common;

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

        
        public async Task<string> ChangeOrderCancel(string token, string orderID)
        {
            var orderAggregate = await orderRepository.getById(token, orderID);
            orderAggregate.update(OrderStateType.CancelPay);           
            await orderRepository.update(orderAggregate);
            return orderAggregate.LogisticID;

        }

       
        public async Task ChangeOrderPaidSuccess(string token, string orderID)
        {
            var orderAggregate = await orderRepository.getById(token, orderID);
            orderAggregate.update(OrderStateType.SuccessPaid);
            await orderRepository.update(orderAggregate);
        }

        public async Task ChangeOrderPaidComplete(string token, string orderID)
        {
            var orderAggregate = await orderRepository.getById(token, orderID);
            orderAggregate.update(OrderStateType.CompletePay);
            await orderRepository.update(orderAggregate);
        }








    }
}
