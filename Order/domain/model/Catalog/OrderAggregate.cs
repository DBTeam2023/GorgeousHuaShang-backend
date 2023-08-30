using Product.common;
using Product.domain.model.repository;
using Product.domain.model.repository.impl;
using Product.dto;
using System.Text.Json.Serialization;

namespace Product.domain.model
{
    public class OrderAggregate
    {
        public string OrderID { get; set; } = null!;

        public string CreateTime { get; set; } = null!;

        public decimal Money { get; set; }

        public int State { get; set; }

        public string LogisticID { get; set; }
        // TODO 存pick 写个返回pick的
        public string StoreID { get; set; }
        public string UserID { get; set; }
        // 名字，电话号码地址一起返回给前端

        public bool? IsDeleted { get; set; } = false;


        internal OrderAggregate() { }

        [JsonConstructor]
        internal OrderAggregate(string orderID, string createTime,
            decimal money, int state, bool? isDeleted,
            string logisticID, string storeID,string userID)
        {

            OrderID = orderID;
            CreateTime = createTime;
            Money = money;
            State = state;
            IsDeleted = isDeleted;
            LogisticID = logisticID;
            StoreID = storeID;
            UserID = userID;
        }

        public static OrderAggregate create(OrderDto order)
        {

            return new OrderAggregate(
                order.OrderId, order.CreateTime,order.Money,order.State,order.IsDeleted,order.LogisticsID,order.StoreID,order.UserID);
        }




    }
}
