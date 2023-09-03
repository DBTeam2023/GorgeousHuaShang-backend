using Order.common;
using Order.domain.model.repository;
using Order.domain.model.repository.impl;
using Order.dto;
using System.Text.Json.Serialization;

namespace Order.domain.model
{
    public class OrderAggregate
    {
        public string OrderID { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public decimal Money { get; set; } 

        public bool State { get; set; } 

        public string LogisticID { get; set; } = null!;
        public string[] PickID { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;


        internal OrderAggregate() { }

        [JsonConstructor]
        internal OrderAggregate(string orderID, DateTime createTime,
           decimal money, bool state, bool isDeleted,
           string logisticID, string[] pickID,string userID,
           string nickName, string phoneNumber, string address)
        {
            OrderID = orderID;
            CreateTime = createTime;
            Money = money;
            State = state;
            IsDeleted = isDeleted;
            LogisticID = logisticID;
            PickID = pickID;
            UserID = userID;
            NickName = nickName;
            PhoneNumber = phoneNumber;
            Address = address;
        }


        public static OrderAggregate Create(OrderDto order)
        {
            return new OrderAggregate(
                order.OrderID, order.CreateTime, order.Money, order.State, order.IsDeleted,
                order.LogisticID, order.PickID, order.UserID,
                order.NickName, order.PhoneNumber, order.Address);
        }

        public static OrderAggregate Create(CreateOrderDto order,BuyerInfoDto buyer)
        {
            var guid = Guid.NewGuid().ToString();
            // 保证id为10位数
            guid = guid.Substring(0, 10);
            // 查询名字，电话号码和地址
            
            return new OrderAggregate(
                guid, order.CreateTime, order.Money, order.State, false,
                order.LogisticID, order.PickID, order.UserID,
                buyer.NickName, buyer.PhoneNumber, buyer.Address);
        }




    }
}
