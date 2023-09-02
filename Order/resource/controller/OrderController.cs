using Microsoft.AspNetCore.Mvc;
using Order.domain.model.repository.impl;
using Order.common;
using Order.domain.model;
using EntityFramework.Context;
using Order.utils;
using Order.application;
using Order.dto;
using Order.resource.vo;
using Product.application;
using Product.resource.vo;

namespace Order.resource.controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        //application service
        public OrderApplicationService OrderApplicationService;
        public OrderController(OrderApplicationService _OrderApplicationService)
        {
            OrderApplicationService = _OrderApplicationService;
        }

        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<OrderIdDto>> createOrder([FromBody] CreateOrderDto order)
        {
            return ComResponse<OrderIdDto>.success(await OrderApplicationService.createOrder(order));
        }

        //Authorization:seller
        [HttpPost]
        public ComResponse<OrderInfoVo> getOrderInfo([FromBody] OrderIdDto orderId)
        {
            var OrderAggrgate = OrderApplicationService.getOrderInfo(orderId.OrderId);
            return ComResponse<OrderInfoVo>.success(new OrderInfoVo(OrderAggrgate));

        }

        //Authorization:seller
        [HttpPost]
        public async Task<ComResponse<string>> updateOrder([FromBody] OrderDto order)
        {
            await OrderApplicationService.UpdateOrder(order);
            return ComResponse<string>.success("成功更新");
        }

        //Authorization:seller
        //对某种商品的删除（全部删除）
        [HttpPost]
        public async Task<ComResponse<string>> deleteOrder([FromBody] OrderIdDto orderID)
        {
            await OrderApplicationService.DeleteOrder(orderID.OrderId);
            return ComResponse<string>.success("成功删除");
        }

        [HttpPost]
        public ComResponse<IPage<OrderVo>> orderPageQuery([FromBody] PageQueryDto pageQuery)
        {
            var page = OrderApplicationService.orderPageQuery(pageQuery);
            return ComResponse<IPage<OrderVo>>.success(OrderVo.CreateOrderPageVo(page));
        }
    }

}

