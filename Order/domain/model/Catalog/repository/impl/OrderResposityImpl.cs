using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Product.common;
using Product.exception;
using Product.utils;
using Product.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFramework.Context;

namespace Product.domain.model.repository.impl
{
    public class OrderRepositoryImpl : OrderRepository
    {
        private readonly ModelContext _context;

        public OrderRepositoryImpl(ModelContext context, CategoryRepository categoryRepository)
        {
            _context = context;
        }

        public async Task Add(OrderAggregate productAggregate)
        {
            // 实现添加订单的逻辑
            _context.Orders.Add(productAggregate);
            await _context.SaveChangesAsync();
        }

        public async Task Update(OrderAggregate productAggregate)
        {
            // 实现更新订单的逻辑
            _context.Orders.Update(productAggregate);
            await _context.SaveChangesAsync();
        }

        public OrderAggregate GetById(string orderId)
        {
            // 实现通过订单ID获取订单的逻辑
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public async Task Delete(string orderId)
        {
            // 实现删除订单的逻辑
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
