using Newtonsoft.Json.Linq;
using Order.dto;
using Order.exception;

namespace Order.resource.remote
{
    public class UserRemote
    {
        public static async Task<string> getUserId(string token)
        {
            string userId;
            string url = "http://47.115.231.142:1025/UserIdentification/getUserInfo";

            HttpClient client = new HttpClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);
                HttpResponseMessage response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                JObject code = JObject.Parse(responseBody);
                Console.WriteLine(code);

                // 获取 userId 字段的值
                userId = (string)code["data"]["userId"];

                Console.WriteLine(userId);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            if (userId == null)
                throw new NotFoundException("Can not find this user.");

            return userId;
        }


        public static async Task<BuyerInfoDto> getUserInfo(string token)
        {
            var user_info = new BuyerInfoDto();

            string url = "http://47.115.231.142:1025/UserIdentification/getUserInfo";

            HttpClient client = new HttpClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);
                HttpResponseMessage response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);

                //Console.WriteLine(code);
                JObject code = JObject.Parse(responseBody);
               

                // 获取 userId 字段的值
                user_info.UserId = (string)code["data"]["userId"];
                //Console.WriteLine(user_info.UserId);
                if ((string)code["data"]["type"]=="buyer")
                    user_info.Address = (string)code["data"]["buyerInfo"]["address"];

                if ((string)code["data"]["type"] == "seller")
                    user_info.Address = (string)code["data"]["sellerInfo"]["address"];
                //Console.WriteLine(user_info.Address);

                user_info.NickName= (string)code["data"]["nickName"]?? (string)code["data"]["username"];
                //Console.WriteLine(user_info.NickName);
                user_info.PhoneNumber= (string)code["data"]["phoneNumber"];
                //Console.WriteLine(user_info.PhoneNumber);


            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }

            if (user_info.UserId == null)
                throw new NotFoundException("Can not find this user.");

            return user_info;
        }




    }
}
