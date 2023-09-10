using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Order.exception;
using System.Text;
using Order.dto;

namespace Order.resource.remote
{
    public class CartRemote
    {
        public static async Task<int> addItemAsync(string token,string pickid,decimal num)
        {
            string url = "http://47.115.231.142:1030/Cart/addItem";
           
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // 设置请求的参数对象
                    var requestData = new { pickId = pickid,number=num  };


                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

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
                    }
                    else
                    {
                        throw new CartException("cart remote failure");
                    }
                }

            }
            catch
            {
                throw new CartException("cart remote failure");
            }
            return 1;
        }




        public static async Task<int> deleteItemsAsync(string token,List<string> pickids)
        {
            string url = "http://47.115.231.142:1030/Cart/deleteItems";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // 设置请求的参数对象
                    List<PickidDto> requestData = new List<PickidDto>();
                    foreach (string pickid in pickids)
                    {
                        requestData.Add(new PickidDto { pickId = pickid, });
                    }

                    // 将参数对象转换为 JSON 字符串
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);
                    Console.WriteLine(jsonContent.ToString());
                    // 设置请求的内容
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // 发送 POST 请求，并将 JSON 字符串作为请求主体传递
                    var response = await httpClient.PostAsync(url, content);
                    Console.WriteLine(response.ToString());

                    // 处理响应
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        JObject code = JObject.Parse(responseContent);

                    }
                    else
                    {
                        throw new CartException("cart remote failure");
                    }
                }

            }
            catch
            {
                throw new CartException("cart remote failure");
            }
            return 1;
        }
    }
}
