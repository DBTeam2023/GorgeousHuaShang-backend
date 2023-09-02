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

        public string CreateTime { get; set; } = null!;

        public decimal Money { get; set; } 

        public int State { get; set; } 

        public string LogisticID { get; set; } = null!;
        public string[] PickID { get; set; } = null!;
        public string StoreID { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

        public bool? IsDeleted { get; set; } = false;


        internal OrderAggregate() { }

        [JsonConstructor]
        internal OrderAggregate(string orderID, string createTime,
           decimal money, int state, bool? isDeleted,
           string logisticID, string[] pickID, string storeID, string userID,
           string nickName, string phoneNumber, string address)
        {
            OrderID = orderID;
            CreateTime = createTime;
            Money = money;
            State = state;
            IsDeleted = isDeleted;
            LogisticID = logisticID;
            PickID = pickID;
            StoreID = storeID;
            UserID = userID;
            NickName = nickName;
            PhoneNumber = phoneNumber;
            Address = address;
        }


        public static OrderAggregate Create(OrderDto order)
        {
            return new OrderAggregate(
                order.OrderID, order.CreateTime, order.Money, order.State, order.IsDeleted,
                order.LogisticID, order.PickID, order.StoreID, order.UserID,
                order.NickName, order.PhoneNumber, order.Address);
        }




    }
}
