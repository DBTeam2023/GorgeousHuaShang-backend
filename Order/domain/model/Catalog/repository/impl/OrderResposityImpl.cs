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
using Microsoft.OpenApi.Any;
using Product.domain.model.repository;

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

            // 首先检查订单是否已存在
            var existingOrder = _context.Orders.FirstOrDefault(o => o.ID == orderAggregate.OrderID);
            if (existingOrder != null)
            {
                throw new DuplicateException("Order already exists.");
            }

            // 创建新的 Order 实例并从 OrderAggregate 对象中初始化数据
            var newOrder = new EntityFramework.Models.Order
            {
                ID = orderAggregate.OrderID,
                Time = orderAggregate.CreateTime,
                Money = orderAggregate.Money,
                State = orderAggregate.State,
                LogisticID = orderAggregate.LogisticID,
                UserID = orderAggregate.UserID,
                IsDeleted = orderAggregate.IsDeleted,
                PickID = orderAggregate.PickID,
            };
            // 添加新订单到数据库
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
        }

        public async Task update(OrderAggregate orderAggregate)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(o => o.ID == orderAggregate.OrderID);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");

            // 更新订单属性
            dbOrder.ID = orderAggregate.OrderID;
            dbOrder.Time = orderAggregate.CreateTime;
            dbOrder.Money = orderAggregate.Money;
            dbOrder.State = orderAggregate.State;
            dbOrder.LogisticID = orderAggregate.LogisticID;
            dbOrder.UserID = orderAggregate.UserID;
            dbOrder.IsDeleted = orderAggregate.IsDeleted;
            dbOrder.PickID = orderAggregate.PickID;

            IDbContextTransaction? transaction = null;
            try
            {
                transaction = _context.Database.BeginTransaction();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                if (transaction != null)
                    await transaction.RollbackAsync();
                throw new DBFailureException("Failed to update the order.");
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public OrderAggregate getById(string orderId)
        {
            var dbOrder = _context.Orders.FirstOrDefault(o => o.ID == orderId);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");

            var orderAggregate = new OrderAggregate
            {
                OrderID = dbOrder.ID,
                CreateTime = dbOrder.Time,
                Money = dbOrder.Money,
                State = dbOrder.State,
                LogisticID = dbOrder.LogisticID,
                UserID = dbOrder.UserID,
                IsDeleted = dbOrder.IsDeleted,
                PickID = dbOrder.PickID,
            };

            return orderAggregate;
        }

        public async Task delete(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.ID == orderId);
            if (order == null)
                throw new NotFoundException("The order doesn't exist.");

            _context.Orders.Remove(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new DBFailureException("Failed to delete the order.");
            }
        }
        private List<OrderAggregate> Convert(List<EntityFramework.Models.Order> orders)
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
                    LogisticID = order.LogisticID,
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
