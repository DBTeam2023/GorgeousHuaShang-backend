using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Order.exception;
using System.Text;

namespace Order.resource.remote
{

    //TODO
    public class StockRemote
    {
        public static async Task reduceStock(string id, decimal num)
        {


            string url = "http://47.115.231.142:1030/Stock/reduceStock";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // 设置请求的参数对象
                    var requestData = new { pickId = id, number = num };

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
                        throw new StockException("not enough stock");
                    }
                }

            }
            catch
            {
                throw new StockException("not enough stock");
            }
            
        }
    }
}
