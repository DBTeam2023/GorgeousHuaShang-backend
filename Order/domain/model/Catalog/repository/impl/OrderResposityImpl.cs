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
using Order.domain.model.repository;

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
            var existingOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderAggregate.OrderID);
            if (existingOrder != null)
            {
                throw new DuplicateException("Order already exists.");
            }

            // 创建新的 Order 实例并从 OrderAggregate 对象中初始化数据
            var newOrder = new EntityFramework.Models.Order
            {
                OrderId = orderAggregate.OrderID,
                CreateTime = orderAggregate.CreateTime,
                Money = orderAggregate.Money,
                State = orderAggregate.State,
                LogisticsId = orderAggregate.LogisticID,
                UserId = orderAggregate.UserID,
                IsDeleted = orderAggregate.IsDeleted,
            };
            // 处理 pick
            for (int i = 0;i< orderAggregate.PickID.Length; ++i)
            {
                var newPicks = new EntityFramework.Models.OrderPick
                {
                    OrderId = orderAggregate.OrderID,
                    PickId = orderAggregate.PickID[i],
                };
                _context.OrderPicks.Add(newPicks);
            }
            // 添加新订单到数据库
            _context.Orders.Add(newOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.ToString());
                }
            }
        }

        public async Task update(OrderAggregate orderAggregate)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderAggregate.OrderID);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");

            // 更新订单属性
            dbOrder.OrderId = orderAggregate.OrderID;
            dbOrder.CreateTime = orderAggregate.CreateTime;
            dbOrder.Money = orderAggregate.Money;
            dbOrder.State = orderAggregate.State;
            dbOrder.LogisticsId = orderAggregate.LogisticID;
            dbOrder.UserId = orderAggregate.UserID;

            // 处理 pick
            for (int i = 0; i < orderAggregate.PickID.Length; ++i)
            {
                var info = await _context.OrderPicks.FirstOrDefaultAsync(u => u.OrderId == orderAggregate.OrderID && u.PickId == orderAggregate.PickID[i]);
                if (info == null)
                {
                    var newPick = new EntityFramework.Models.OrderPick
                    {
                        OrderId = orderAggregate.OrderID,
                        PickId = orderAggregate.PickID[i],
                    };
                    _context.OrderPicks.Add(newPick);
                }
                else
                {
                    // 从上下文中删除数据行
                    _context.OrderPicks.Remove(info);
                }
            }

            // 异步保存更改到数据库
            await _context.SaveChangesAsync();
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
            var dbOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");
            var picks = _context.OrderPicks
                .Where(u => u.OrderId == orderId)
                .Select(u => u.PickId)
                .ToArray();
            var orderAggregate = new OrderAggregate
            {
                OrderID = dbOrder.OrderId,
                CreateTime = dbOrder.CreateTime,
                Money = dbOrder.Money,
                State = dbOrder.State,
                LogisticID = dbOrder.LogisticsId,
                UserID = dbOrder.UserId,
                IsDeleted = dbOrder.IsDeleted,
                PickID = picks
            };
            return orderAggregate;
        }

        public async Task delete(string orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new NotFoundException("The order doesn't exist.");

            _context.Orders.Remove(order);
            var picks = _context.OrderPicks.Where(u => u.OrderId == orderId);
            _context.OrderPicks.RemoveRange(picks);
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
                    OrderID = order.OrderId,
                    CreateTime = order.CreateTime,
                    Money = order.Money,
                    State = order.State,
                    LogisticID = order.LogisticsId,
                    UserID = order.UserId,
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
                .Where(x => x.OrderId == (pageQuery.OrderId ?? x.OrderId))
                .Where(x => x.UserId == (pageQuery.UserID ?? x.UserId))
                .Where(x => x.Money >= (pageQuery.Moneymin ?? decimal.MinValue))
                .Where(x => x.Money <= (pageQuery.Moneymax ?? decimal.MaxValue))
                //.Where(x => x.PickID == (pageQuery.CommodityId ?? x.PickID))
                // TODO .Where(x => x.PickID.Contains(pageQuery.CommodityId ?? ""))
                .Where(x => x.Money >= (pageQuery.TotalAmount ?? decimal.MinValue))
                .Where(x => x.State == (pageQuery.OrderStatus ?? x.State))
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
