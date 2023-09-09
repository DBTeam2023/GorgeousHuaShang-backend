using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Product.application;

namespace Product.utils
{
    public class StockReleaseMQListener : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
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
                    channel.QueueDeclare(queue: "stock_release_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    channel.QueueBind(queue: "stock_release_queue", exchange: "stock_event_exchange", routingKey: "stock.release");
                    var ReleaseConsumer = new EventingBasicConsumer(channel);
                    ReleaseConsumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var eventData = JsonConvert.DeserializeObject(message);

                        // 处理接收到的消息
                        release(eventData);

                        // 手动确认消息已被处理
                        //channel.BasicAck(ea.DeliveryTag, false);

                    };
                    channel.BasicConsume(queue: "stock_release_queue", autoAck: true, consumer: ReleaseConsumer);

                    // execution delay
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }   
            }
        }

        private void release(object eventData)
        {
            // 在这里处理接收到的消息
            Console.WriteLine("release：" + eventData.ToString());
        }

    }

}
