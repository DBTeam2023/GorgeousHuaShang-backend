using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Payment.message
{
    public class RabbitMQMessageReceiver
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;

        public RabbitMQMessageReceiver(string hostName, string queueName)
        {
            _factory = new ConnectionFactory() { HostName = hostName };
            _queueName = queueName;
        }

        public void StartReceiving()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventData = JsonConvert.DeserializeObject(message);

                    // 处理接收到的消息
                    HandleMessage(eventData);

                    // 手动确认消息已被处理
                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

                Console.WriteLine("等待接收消息...");
                Console.ReadLine();
            }
        }

        private void HandleMessage(object eventData)
        {
            // 在这里处理接收到的消息
            Console.WriteLine("接收到消息：" + eventData.ToString());
        }
    }

}
