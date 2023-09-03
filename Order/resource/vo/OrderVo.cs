using Order.domain.model;
using Order.utils;


namespace Order.resource.vo
{
    public class OrderVo
    {
        public string OrderId { get; set; } = null!;
        public string CreateTime { get; set; } = null!;
        public decimal Money { get; set; }
        public int State { get; set; }
        public string LogisticId { get; set; }
        public string StoreId { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }

        public OrderVo(OrderAggregate orderAggregate)
        {
            OrderId = orderAggregate.OrderID;
            CreateTime = orderAggregate.CreateTime;
            Money = orderAggregate.Money;
            State = orderAggregate.State;
            LogisticId = orderAggregate.LogisticID;
            UserId = orderAggregate.UserID;
            IsDeleted = orderAggregate.IsDeleted ?? false;
        }

        public static IPage<OrderVo> CreateOrderPageVo(IPage<OrderAggregate> orderAggregatePage)
        {
            var orderVoList = new List<OrderVo>();
            foreach (var order in orderAggregatePage.Records)
                orderVoList.Add(new OrderVo(order));

            return IPage<OrderVo>.builder()
                .records(orderVoList)
                .size(orderAggregatePage.Size)
                .total(orderAggregatePage.Total)
                .current(orderAggregatePage.Current)
                .build();
        }
    }
}

