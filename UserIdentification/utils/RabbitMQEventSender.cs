using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
namespace Payment.message
{
    public class RabbitMQEventSender
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private bool _queueDeclared;

        public RabbitMQEventSender(string hostName, string queueName, string userName, string password)
        {
            _factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3000) 
            };
            _queueName = queueName;
            _queueDeclared = false;
        }

        public void SendEvent(object eventData)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                if (!_queueDeclared)
                {
                    //try
                    //{
                    //    channel.QueueDeclarePassive(_queueName);
                    //    Console.WriteLine("queue exists");
                    //    _queueDeclared = true;
                    //}
                    //catch (Exception)
                    //{
                    //    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    //    Console.WriteLine("queue declared");
                    //    _queueDeclared = true;
                    //}
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine("queue declared");
                    _queueDeclared = true;
                }

                var message = JsonConvert.SerializeObject(eventData);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
        }
    }
}
