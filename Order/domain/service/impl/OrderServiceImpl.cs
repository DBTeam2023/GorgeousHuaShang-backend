using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Order.domain.model;
using Order.domain.model.repository;
using Order.dto;
using Order.exception;
using Order.resource.vo;

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

        public BuyerInfoDto getBuyerInfo(string userID)
        {
            var user = modelContext.Users.FirstOrDefault(u => u.UserId == userID);
            var buyer = modelContext.Buyers.FirstOrDefault(u => u.UserId == userID);
            var buyerInfo = new BuyerInfoDto();
            if (buyer != null && user != null)
            {
                buyerInfo = new BuyerInfoDto
                {
                    NickName = user.NickName ?? "NULL",
                    PhoneNumber = user.PhoneNumber ?? "NULL",
                    Address = buyer.ReceiveAddress ?? "NULL",
                };
            }
            else
            {
                throw new NotFoundException("找不到用户信息");
            }
            return buyerInfo;
        }

    }
}
