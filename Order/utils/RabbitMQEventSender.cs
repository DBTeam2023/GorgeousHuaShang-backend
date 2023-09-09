using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
namespace Product.utils
{
    public class RabbitMQEventSender
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private bool _queueDeclared;

        public RabbitMQEventSender(string queueName)
        {
            _factory = new ConnectionFactory()
            {
                HostName = "47.115.231.142",
                UserName = "admin",
                Password = "123",
                Port = 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3000)
            };
            _queueName = queueName;
        }

        public void sendEvent(object eventData,string key)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "order_event_exchange", type: ExchangeType.Topic, durable: true);
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: _queueName, exchange: "order_event_exchange", routingKey: key);
                var message = JsonConvert.SerializeObject(eventData);
                var body = Encoding.UTF8.GetBytes(message);
                Console.WriteLine(message);    
                channel.BasicPublish(exchange: "order_event_exchange", routingKey: key, basicProperties: null, body: body);
            }
        }

        public void sendDelayedEvent(object eventData, string key, int delayMilliseconds = 30 * 60 * 1000)
        {

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // 创建原始队列
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>
                {
                    { "x-message-ttl", delayMilliseconds },
                    { "x-dead-letter-exchange", "" },  // 空字符串表示使用默认交换机
                    { "x-dead-letter-routing-key", "delayed_routing_key" }  // 延迟消息过期后将被重新发布的路由键
                 });
                channel.QueueBind(queue: _queueName, exchange: "order_event_exchange", routingKey: key);

                // 将消息发送到原始队列
                var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventData));
                channel.BasicPublish(exchange: "order_event_exchange", routingKey: key, basicProperties: null, body: messageBytes);
            }
        }
    }
}
