using Order.common;
using Order.domain.model.Order;
using Order.domain.model.repository;
using Order.domain.model.repository.impl;
using Order.dto;
using Order.exception;
using Order.resource.remote;
using System.Text.Json.Serialization;

namespace Order.domain.model
{
  
    public class OrderAggregate
    {
        public string OrderID { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        public decimal Money { get; set; }

        public string State { get; set; } = null!;

        public string LogisticID { get; set; } = null!;


        public List<DPick> Picks { get; set; } = null!;
      


        public string UserID { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

       


        internal OrderAggregate() { }

        [JsonConstructor]
        internal OrderAggregate(string orderID, DateTime createTime,
           decimal money, string state,string logisticID, List<DPick>picks,
           string userID,string nickName, string phoneNumber, string address)
        {
            OrderID = orderID;
            CreateTime = createTime;
            Money = money;
            State = state;
            LogisticID = logisticID;
            Picks = picks;
            UserID = userID;
            NickName = nickName;
            PhoneNumber = phoneNumber;
            Address = address;
        }


        public static async Task<OrderAggregate> Create(BuyerInfoDto user_info, List<DPick> picks)
        {
            string order_id=Guid.NewGuid().ToString();
           
            if (user_info.Address == null)
                throw new NotFoundException("亲，请先在个人中心完善你的地址哟~");

            //判断库存够不够



            string logisticID = await LogisticsRemote.addLogistics(user_info.Address);
            decimal money = 0;
            

            foreach (var pick in picks)
                money += (pick.Number * pick.Price);


            return new OrderAggregate(
                order_id,DateTime.Now, money,OrderStateType.WaitToPay, logisticID,
                picks, user_info.UserId, user_info.NickName,
                user_info.PhoneNumber, user_info.Address);
        }

        public void update(string state)
        {
            OrderStateType.TypeCheck(state);

            if (state==OrderStateType.CancelPay)
            {
                if (this.State == OrderStateType.SuccessPaid|| this.State == OrderStateType.CompletePay)
                    throw new OrderStateException("已支付，不可以取消订单！");
                else
                    this.State = OrderStateType.CancelPay;
                return;
            }

            else if (state == OrderStateType.SuccessPaid)
            {
                if (this.State == OrderStateType.CancelPay)
                    throw new OrderStateException("已取消，请重新生成订单！");
                else if(this.State == OrderStateType.CompletePay)
                    throw new OrderStateException("订单已完成！");
                else
                    this.State = OrderStateType.SuccessPaid;
                return;
            }
            else if(state== OrderStateType.WaitToPay)
            {
                if (this.State == OrderStateType.CancelPay)
                    throw new OrderStateException("已取消，请重新生成订单！");
                else if (this.State == OrderStateType.SuccessPaid)
                    throw new OrderStateException("已支付");
                else if (this.State == OrderStateType.CompletePay)
                    throw new OrderStateException("订单已完成！");
                return;
            }

            else
            {
                if (this.State == OrderStateType.CancelPay)
                    throw new OrderStateException("已取消的订单");
                else if (this.State == OrderStateType.WaitToPay)
                    throw new OrderStateException("尚未付款");
                else
                    this.State = OrderStateType.CompletePay;
            }
           
            
         
        }

    }
}
