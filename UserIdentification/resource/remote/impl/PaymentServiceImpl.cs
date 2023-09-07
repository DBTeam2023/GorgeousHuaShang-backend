using  UserIdentification.dto;
using Newtonsoft.Json.Linq;
using UserIdentification.exception;
using Newtonsoft.Json;
using System.Text;
namespace UserIdentification.resource.remote.impl
{
    public class PaymentServiceImpl: PaymentService
    {
        public async Task addWallet(TokenDto tokenDto, decimal balance)
        {
            // send http request
            if (tokenDto.Token == null)
                throw new NotFoundException("This token is null.");
            string url = "http://47.115.231.142:1025/UserIdentification/getUserInfo";

            HttpClient client = new HttpClient();
            try
            {
                var requestData = new { balance = 100000 };
                string requestBody = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", tokenDto.Token);
                request.Content = content;

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                JObject code = JObject.Parse(responseBody);
                Console.WriteLine(code);

            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
