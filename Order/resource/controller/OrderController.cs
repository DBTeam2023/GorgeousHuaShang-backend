using Microsoft.AspNetCore.Mvc;
using Order.domain.model.repository.impl;
using Order.common;
using Order.domain.model;
using EntityFramework.Context;
using Order.utils;
using Order.application;
using Order.dto;
using Order.resource.remote;

namespace Order.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        //application service
        public OrderApplicationService orderApplicationService;
        public OrderController(OrderApplicationService _orderApplicationService)
        {
            orderApplicationService = _orderApplicationService;
        }

        //ok
        [HttpPost]
        public async Task<ComResponse<OrderAggregate>> createOrder([FromBody] CreateOrderDto order)
        {
           string token = HttpContext.Request.Headers["Authorization"];           
           return ComResponse<OrderAggregate>.success(await orderApplicationService.createOrder(token, order));
        }


        [HttpPost]
        public async Task<ComResponse<OrderAggregate>> getOrderInfo([FromBody] OrderIdDto orderId)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var orderAggrgate = await orderApplicationService.getOrderInfo(token,orderId.OrderId);
            return ComResponse<OrderAggregate>.success(orderAggrgate);
        }



        
        [HttpPost]
        public async Task<ComResponse<string>> deleteOrder([FromBody] OrderIdDto orderID)
        {
            
            await orderApplicationService.DeleteOrder(orderID.OrderId);
            return ComResponse<string>.success("成功删除");
        }

        //取消订单
        [HttpPost]
        public async Task<ComResponse<int>> ChangeOrderCancel([FromBody] OrderIdDto orderID)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            await orderApplicationService.ChangeOrderCancel(token,orderID.OrderId);
            return ComResponse<int>.success(0);
        }

       

        

        [HttpPost]
        public async Task<ComResponse<int>> ChangeOrderPaidSuccess([FromBody] OrderIdDto orderID)
        {

            string token = HttpContext.Request.Headers["Authorization"];
            await orderApplicationService.ChangeOrderPaidSuccess(token, orderID.OrderId);
            return ComResponse<int>.success(0);
        }

        [HttpPost]
        public async Task<ComResponse<int>> ChangeOrderPaidComplete([FromBody] OrderIdDto orderID)
        {

            string token = HttpContext.Request.Headers["Authorization"];
            await orderApplicationService.ChangeOrderPaidComplete(token, orderID.OrderId);
            return ComResponse<int>.success(0);
        }




        [HttpPost]
        public async Task<ComResponse<IPage<OrderAggregate>>> orderPageQuery([FromBody] PageQueryDto pageQuery)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var page = await orderApplicationService.orderPageQuery(token,pageQuery);
            return ComResponse<IPage<OrderAggregate>>.success(page);
        }






    }

}

