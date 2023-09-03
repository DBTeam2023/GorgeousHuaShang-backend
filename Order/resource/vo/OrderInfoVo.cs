using Order.common;
using Order.domain.model;

namespace Order.domain.model
{
    public class OrderInfoVo
    {
        public string OrderId { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public decimal Money { get; set; }
        public bool State { get; set; }
        public string LogisticId { get; set; } = null!;
        public string[] PickId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

        public OrderInfoVo(OrderAggregate orderAggregate)
        {
            OrderId = orderAggregate.OrderID;
            CreateTime = orderAggregate.CreateTime;
            Money = orderAggregate.Money;
            State = orderAggregate.State;
            LogisticId = orderAggregate.LogisticID;
            PickId = orderAggregate.PickID;
            UserId = orderAggregate.UserID;
            NickName = orderAggregate.NickName;
            PhoneNumber = orderAggregate.PhoneNumber;
            Address = orderAggregate.Address;
            IsDeleted = orderAggregate.IsDeleted;
        }
    }
}