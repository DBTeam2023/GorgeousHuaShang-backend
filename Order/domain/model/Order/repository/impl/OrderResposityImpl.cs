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
using Order.domain.model.Order;
using Order.resource.remote;

namespace Order.domain.model.repository.impl
{
    public class OrderRepositoryImpl : OrderRepository
    {
        private readonly ModelContext _context;

        public OrderRepositoryImpl(ModelContext context)
        {
            _context = context;
        }

        //ok
        public async Task add(OrderAggregate orderAggregate)
        {
            // 实现添加订单的逻辑

            // 首先检查订单是否已存在
            var existingOrder = _context.Myorders.FirstOrDefault(o => o.OrderId == orderAggregate.OrderID);
            if (existingOrder != null)
            {
                throw new DuplicateException("Order already exists.");
            }

            // 创建新的 Order 实例并从 OrderAggregate 对象中初始化数据
            var newOrder = new Myorder
            {
                OrderId = orderAggregate.OrderID,
                CreateTime = orderAggregate.CreateTime,
                Money = orderAggregate.Money,                
                LogisticsId = orderAggregate.LogisticID,
                UserId = orderAggregate.UserID,
                State=orderAggregate.State
            };

            _context.Myorders.Add(newOrder);

            // 处理 pick

            foreach(var it in orderAggregate.Picks)
            {
                var newPicks = new EntityFramework.Models.OrderPick
                {
                    PickId=it.PickId,
                    Number=it.Number,
                    OrderId= orderAggregate.OrderID
                };
                _context.OrderPicks.Add(newPicks);
            }
                  
            try
            {
                await _context.SaveChangesAsync();
            }
            catch 
            {
                throw new DBFailureException("add order failure");
            }
        }

        //偷懒：只修改state
        public async Task update(OrderAggregate orderAggregate)
        {
            var dbOrder = await _context.Myorders.FirstOrDefaultAsync(o => o.OrderId == orderAggregate.OrderID);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");
          
            dbOrder.State = orderAggregate.State;
            try
            {
                // 异步保存更改到数据库
                await _context.SaveChangesAsync();

            }
            catch
            {
                throw new DBFailureException("update order failure");
            }


        }



        //ok
        public async Task<OrderAggregate> getById(string token,string orderId)
        {
            var dbOrder = _context.Myorders.FirstOrDefault(o => o.OrderId == orderId);
            if (dbOrder == null)
                throw new NotFoundException("The order doesn't exist.");
            

            var picks = _context.OrderPicks
                .Where(u => u.OrderId == orderId)
                .Select(u => new CreateOrderDto.CreateOrder{PickId=u.PickId,Number=u.Number})
                .ToList();

            
            var picks_in_aggregate = await PickRemote.getPickInfos(new CreateOrderDto
            {
                OrderCreate = picks,
            });

            var user_info = await UserRemote.getUserInfo(token);
                
            var orderAggregate = new OrderAggregate
            {
                OrderID = dbOrder.OrderId,
                CreateTime = dbOrder.CreateTime,
                Money = dbOrder.Money,
                State = dbOrder.State,
                LogisticID = dbOrder.LogisticsId,
                UserID = dbOrder.UserId,
                Picks= picks_in_aggregate,
                Address=user_info.Address,
                NickName=user_info.NickName,
                PhoneNumber=user_info.PhoneNumber
            };
            return orderAggregate;
        }


        //ok
        public async Task delete(string orderId)
        {
            var order = _context.Myorders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new NotFoundException("The order doesn't exist.");

            _context.Myorders.Remove(order);

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


       
        

        public async Task<IPage<OrderAggregate>> pageQuery(string token,PageQueryDto pageQuery)
        {
            // 检查查询参数的有效性
            pageQuery.Check();

            // 获取所有订单记录
            var allOrders = _context.Myorders.ToList();

            var user_info = await UserRemote.getUserInfo(token);

            // 根据查询参数逐步过滤订单记录
            var filteredOrders = allOrders
                .Where(x => x.OrderId == (pageQuery.OrderId ?? x.OrderId))
                .Where(x => x.UserId == (user_info.UserId ?? x.UserId))
                .Where(x => x.Money >= (pageQuery.Moneymin ?? decimal.MinValue))
                .Where(x => x.Money <= (pageQuery.Moneymax ?? decimal.MaxValue))
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

            var orderAggregateList = new List<OrderAggregate>();
            foreach (var it in filteredOrders)
                orderAggregateList.Add(await getById(token, it.OrderId));




            // 构建分页结果对象
            var page = IPage<OrderAggregate>.builder()
                .total(total)
                .size(pageSize)
                .current(pageIndex)
                .records(orderAggregateList)
                .build();

            return page;

        }


    }
}
