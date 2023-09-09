using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Order.application;

namespace Order.utils
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
                    channel.QueueDeclare(queue: "order_release_order_queue",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    channel.QueueBind(queue: "order_release_order_queue", exchange: "order_event_exchange", routingKey: "order.cancel");
                    var StockReleaseConsumer = new EventingBasicConsumer(channel);
                    StockReleaseConsumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var eventData = JsonConvert.DeserializeObject(message);

                        // 处理接收到的消息
                        stockRelease(eventData);

                    };
                    channel.BasicConsume(queue: "stock_release_queue", autoAck: true, consumer: StockReleaseConsumer);

                    // execution delay
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private void stockRelease(object eventData)
        {
            // 在这里处理接收到的消息
            Console.WriteLine("stockRelease：" + eventData.ToString());
        }

    }

}