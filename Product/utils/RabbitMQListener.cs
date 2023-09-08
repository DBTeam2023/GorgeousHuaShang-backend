using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;
using Product.application;

namespace Product.utils
{
    public class RabbitMQListener : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ConnectionFactory _factory;
        public RabbitMQListener()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "47.115.231.142",
                UserName = "admin",
                Password = "123",
                Port = 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3000)
            };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = _factory.CreateConnection();
            // 进行消息监听的相关逻辑，参考之前的示例代码
            using (var DelayChannel = connection.CreateModel())
            using (var ReleaseChannel = connection.CreateModel())
            using (var ReduceChannel = connection.CreateModel())
            {
                var args = new Dictionary<string, object>
                {
                    { "x-message-ttl", 20000 },
                    { "x-dead-letter-exchange", "stock_event_exchange" }, // 空字符串表示使用默认交换机
                    { "x-dead-letter-routing-key", "stock.release" } // 延迟消息过期后将被重新发布的路由键
                 };

                DelayChannel.QueueDeclare(queue: "stock_delay_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
                ReleaseChannel.QueueDeclare(queue: "stock_release_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                ReduceChannel.QueueDeclare(queue: "stock_reduce_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                ReduceChannel.ExchangeDeclare(exchange: "stock_event_exchange", type: ExchangeType.Topic);
                ReduceChannel.QueueBind(queue: "stock_reduce_queue", exchange: "stock_event_exchange", routingKey: "");
                //var DelayConsumer = new AsyncEventingBasicConsumer(DelayChannel);
                //Console.WriteLine("delay");
                //DelayConsumer.Received += async (model, ea) =>
                //{
                //    var body = ea.Body.ToArray();
                //    var message = Encoding.UTF8.GetString(body);
                //    var eventData = JsonConvert.DeserializeObject(message);

                //    // 处理接收到的消息
                //    HandleMessage(eventData);

                //    // 手动确认消息已被处理
                //    DelayChannel.BasicAck(ea.DeliveryTag, false);

                //    // 允许其他进程执行
                //    await Task.Yield();
                //};

                //var ReleaseConsumer = new AsyncEventingBasicConsumer(ReleaseChannel);
                //Console.WriteLine("release");
                //ReleaseConsumer.Received += async (model, ea) =>
                //{
                //    var body = ea.Body.ToArray();
                //    var message = Encoding.UTF8.GetString(body);
                //    var eventData = JsonConvert.DeserializeObject(message);

                //    // 处理接收到的消息
                //    HandleMessage(eventData);

                //    // 手动确认消息已被处理
                //    ReleaseChannel.BasicAck(ea.DeliveryTag, false);

                //    // 允许其他进程执行
                //    await Task.Yield();
                //};

                var ReduceConsumer = new AsyncEventingBasicConsumer(ReduceChannel);
               
                ReduceConsumer.Received += async (model, ea) =>
                {
                    Console.WriteLine("reduce");
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventData = JsonConvert.DeserializeObject(message);
                    // 处理接收到的消息
                    HandleMessage(eventData);

                    // 手动确认消息已被处理
                    ReduceChannel.BasicAck(ea.DeliveryTag, false);

                    // 允许其他进程执行
                    await Task.Yield();
                };

                //DelayChannel.BasicConsume(queue: "stock_delay_queue", autoAck: false, consumer: DelayConsumer);
                //ReleaseChannel.BasicConsume(queue: "stock_release_queue", autoAck: false, consumer: ReleaseConsumer);
                ReduceChannel.BasicConsume(queue: "stock_reduce_queue", autoAck: false, consumer: ReduceConsumer);

            }
        }

       

        private void HandleMessage(object eventData)
        {
            // 在这里处理接收到的消息
            Console.WriteLine("接收到消息：" + eventData.ToString());
        }
    }

}
