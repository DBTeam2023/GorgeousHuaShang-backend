using UserIdentification.dto;
using Newtonsoft.Json.Linq;
using UserIdentification.exception;
using Newtonsoft.Json;
using System.Text;
namespace UserIdentification.resource.remote.impl
{
    public class PaymentServiceImpl : PaymentService
    {
        public void addWallet(TokenDto tokenDto, decimal balance)
        {
            // send http request
            if (tokenDto.Token == null)
                throw new NotFoundException("This token is null.");
            string url = "http://47.115.231.142:8081/api/Payment/Wallet/add/";

            HttpClient client = new HttpClient();
            try
            {
                var requestData = new { balance = balance };
                string requestBody = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", tokenDto.Token);
                request.Content = content;

                HttpResponseMessage response = client.Send(request);
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
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
