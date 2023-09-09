using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Order.exception;
using System.Text;

namespace Order.resource.remote
{
    public class LogisticsRemote
    {
        public static async Task<string> addLogistics(string address)
        {
     

            string url = "http://47.115.231.142:1026/Logistics/addLogistics/";
            string? logisticsId = null;
            try
            {         
                using (var httpClient = new HttpClient())
                {
                    // 设置请求的参数对象
                    var requestData = new { company = "华商异彩", shipAddress="上海", pickAddress= address };

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
                        logisticsId = (string)code["data"]["logisticsId"];             
                    }
                    else
                    {
                        throw new LogisticsException("add logistics failure");
                    }
                }
                
            }
            catch
            {
                throw new LogisticsException("add logistics failure");
            }
            return logisticsId;
        }


    }
}
