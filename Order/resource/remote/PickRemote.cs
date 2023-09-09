using EntityFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Order.domain.model.Order;
using Order.dto;
using Order.exception;
using System.Text;

namespace Order.resource.remote
{
    public class PickRemote
    {
        public static async Task<List<DPick>> getPickInfos(CreateOrderDto orderCreate)
        {


            string url = "http://47.115.231.142:1030/Product/getSinglePick/";
            var picks = new List<DPick>();
            try
            {
                foreach (var it in orderCreate.OrderCreate)
                {
                    using (var httpClient = new HttpClient())
                    {
                        // 设置请求的参数对象
                        var requestData = new { pickId = it.PickId };

                        // 将参数对象转换为 JSON 字符串
                        var jsonContent = JsonConvert.SerializeObject(requestData);

                        // 设置请求的内容
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        // 发送 POST 请求，并将 JSON 字符串作为请求主体传递
                        var response = await httpClient.PostAsync(url, content);


                        // 处理响应
                        if (response.IsSuccessStatusCode)
                        {

                            var responseContent = await response.Content.ReadAsStringAsync();
                            JObject code = JObject.Parse(responseContent);
                            
                            var new_pick = new DPick()
                            {
                                CommodityId = (string)code["data"]["commodityId"],
                                Description = (string)code["data"]["commodityInfo"][0]["description"],
                                Number = it.Number,
                                PickId = it.PickId,
                                
                                //PickImage = new FileContentResult(Encoding.UTF8.GetBytes((string)code["data"]["commodityInfo"][0]["image"]["fileContents"]),
                                //(string)code["data"]["commodityInfo"][0]["image"]["contentType"]),
                                Image= (string)code["data"]["commodityInfo"][0]["image"]["fileContents"],
                                ImageType= (string)code["data"]["commodityInfo"][0]["image"]["contentType"],



                                Price = (decimal)code["data"]["commodityInfo"][0]["price"],
                                Property = code["data"]["commodityInfo"][0]["property"].ToObject<Dictionary<string, string>>(),
                            };
                            picks.Add(new_pick);


                        }
                        else
                        {
                            throw new FindPickException("pick not found");
                        }
                    }
                }
            }
            catch
            {
                throw new FindPickException("pick not found");
            }

            return picks;




        }

     


        




    }
}
