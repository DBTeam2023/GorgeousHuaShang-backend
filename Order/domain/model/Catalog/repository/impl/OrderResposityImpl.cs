using EntityFramework.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Order.common;
using Order.exception;
using Order.utils;
using Order.dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EntityFramework.Context;

using Order.utils;
using Microsoft.OpenApi.Any;

namespace Order.domain.model.repository.impl
{
    public class OrderRepositoryImpl : OrderRepository
    {
        private readonly ModelContext _context;

        public OrderRepositoryImpl(ModelContext context)
        {
            _context = context;
        }

        public async Task add(OrderAggregate orderAggregate)
        {
            // 实现添加订单的逻辑
            _context.Orders.Add(orderAggregate);
            await _context.SaveChangesAsync();
        }

        public async Task update(OrderAggregate orderAggregate)
        {
            // 实现更新订单的逻辑
            _context.Orders.Update(orderAggregate);
            await _context.SaveChangesAsync();
        }

        public OrderAggregate getById(string orderId)
        {
            // 实现通过订单ID获取订单的逻辑
            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public async Task delete(string orderId)
        {
            // 实现删除订单的逻辑
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        private List<OrderAggregate> Convert(List<Order> orders)
        {
            var ans = new List<OrderAggregate>();

            foreach (var order in orders)
            {
                var orderAggregate = new OrderAggregate
                {
                    OrderID = order.ID,
                    CreateTime = order.Time,
                    Money = (decimal)order.Money,
                    State = order.State,
                    LogisticID = order.LogisticsID,
                    StoreID = order.StoreID,
                    UserID = order.UserID,
                    IsDeleted = order.IsDeleted
                };

                ans.Add(orderAggregate);

            }

            return ans;
        }
        
        public IPage<OrderAggregate> pageQuery(PageQueryDto pageQuery)
        {
            // 检查查询参数的有效性
            pageQuery.Check();

            // 获取所有订单记录
            var allOrders = _context.Orders.ToList();

            // 根据查询参数逐步过滤订单记录
            var filteredOrders = allOrders
                .Where(x => x.OrderID == (pageQuery.OrderId ?? x.OrderID))
                .Where(x => x.UserID == (pageQuery.UserID ?? x.UserID))
                .Where(x => x.Money >= (pageQuery.Moneymin ?? decimal.MinValue))
                .Where(x => x.Money <= (pageQuery.Moneymax ?? decimal.MaxValue))
                .Where(x => x.CommodityId == (pageQuery.CommodityId ?? x.CommodityId))
                .Where(x => x.TotalAmount >= (pageQuery.TotalAmount ?? decimal.MinValue))
                .Where(x => x.OrderStatus == (pageQuery.OrderStatus ?? x.OrderStatus))
            .ToList();



            // 计算总记录数
            int total = filteredOrders.Count();

            // 分页处理
            int pageIndex = pageQuery.PageIndex;
            int pageSize = pageQuery.PageSize;

            if (total >= (pageIndex - 1) * pageSize + 1)
            {
                if (total <= pageIndex * pageSize)
                {
                    filteredOrders = filteredOrders.GetRange((pageIndex - 1) * pageSize, total - (pageIndex - 1) * pageSize);
                }
                else
                {
                    filteredOrders = filteredOrders.GetRange((pageIndex - 1) * pageSize, pageSize);
                }
            }
            else
            {
                // 如果请求的页数超出范围，抛出异常
                if (!(total == 0 && pageIndex == 1))
                {
                    throw new PageException("Page not found");
                }
            }

            // 构建分页结果对象
            var page = IPage<OrderAggregate>.builder()
                .total(total)
                .size(pageSize)
                .current(pageIndex)
                .records(Convert(filteredOrders))
                .build();

            return page;

        }


    }
}
