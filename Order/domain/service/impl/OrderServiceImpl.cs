using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Order.domain.model;
using Order.domain.model.repository;
using Order.dto;
using Order.exception;
using Order.resource.vo;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;

namespace Order.domain.service.impl
{
    public class OrderServiceImpl : OrderService
    {
        private readonly ModelContext modelContext;
        public OrderRepository orderRepository;
        private string _token = 
        public OrderServiceImpl(ModelContext _modelContext, OrderRepository _orderRepository)
        {
            modelContext = _modelContext;
            orderRepository = _orderRepository;
        }

        public BuyerInfoDto getBuyerInfo(string userID)
        {
            var user = modelContext.Users.FirstOrDefault(u => u.UserId == userID);
            var buyer = modelContext.Buyers.FirstOrDefault(u => u.UserId == userID);
            var buyerInfo = new BuyerInfoDto();
            if (buyer != null && user != null)
            {
                buyerInfo = new BuyerInfoDto
                {
                    NickName = user.NickName ?? "NULL",
                    PhoneNumber = user.PhoneNumber ?? "NULL",
                    Address = buyer.ReceiveAddress ?? "NULL",
                };
            }
            else
            {
                throw new NotFoundException("找不到用户信息");
            }
            return buyerInfo;
        }

        public async Task<PickInfoDto[]> getPickInfos(string[] pickID)
        {
            string commodityID; 
            string url = "http://47.115.231.142:1030/Product/getSinglePick/";
            var pickInfos = new PickInfoDto[pickID.Length];
            Dictionary<string, string> dic;

            HttpClient client = new HttpClient();
            for (int i = 0; i < pickID.Length; ++i)
            {

                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                    // TODO request.Headers.Add("Authorization", token);
                    HttpResponseMessage response = await client.SendAsync(request);

                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);

                    JObject code = JObject.Parse(responseBody);

                    commodityID = (string)code["data"]["CommodityId"];
                    dic = code["data"]["CommodityInfo"]["Property"].ToObject<Dictionary<string, string> > ();
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception(ex.Message);
                }
                if (commodityID == null|| dic==null)
                    throw new NotFoundException("No result");
                var single = new PickInfoDto
                {
                    PickID = pickID[i],
                    CommodityID = commodityID,
                    Property = dic,
                };
                pickInfos[i] = single;
            }

            return pickInfos;
        }

    }
}
