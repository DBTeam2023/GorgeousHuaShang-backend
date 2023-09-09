using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Product.application;
namespace Product.utils
{
    public class StockDelayMQListener: Microsoft.Extensions.Hosting.BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // execution
                // initialization for mq connection
                var factory = new ConnectionFactory()
                {
                    HostName = "47.115.231.142",
                    UserName = "admin",
                    Password = "123",
                    Port = 5672,
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(30000)
                };
                var connection = factory.CreateConnection();

                using (var channel = connection.CreateModel())
                {
                    var args = new Dictionary<string, object>
                    {
                        //{ "x-message-ttl", 50*60*1000 },
                        { "x-message-ttl", 10 },
                        { "x-dead-letter-exchange", "stock_event_exchange" }, // 空字符串表示使用默认交换机
                        { "x-dead-letter-routing-key", "stock.release" } // 延迟消息过期后将被重新发布的路由键
                     };

                    channel.QueueDeclare(queue: "stock_delay_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
                    // channel.QueueBind(queue:)
                    var DelayConsumer = new EventingBasicConsumer(channel);
                    DelayConsumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var eventData = JsonConvert.DeserializeObject(message);

                        // 处理接收到的消息
                        delay(eventData);

                        // 手动确认消息已被处理
                        channel.BasicAck(ea.DeliveryTag, false);

                        // 允许其他进程执行
                        await Task.Yield();
                    };

                    // execution delay
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private void delay(object eventData)
        {
            // 在这里处理接收到的消息
            Console.WriteLine("Delay：" + eventData.ToString());
        }

    }
}
