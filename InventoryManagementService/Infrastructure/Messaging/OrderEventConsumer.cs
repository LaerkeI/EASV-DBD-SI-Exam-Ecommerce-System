using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using InventoryManagementService.Infrastructure.Messaging.Events;
using InventoryManagementService.Application.Interfaces;

namespace InventoryManagementService.Infrastructure.Messaging
{
    public class OrderEventConsumer : BackgroundService
    {
        private readonly string _hostName = "rabbitmq"; // RabbitMQ server host. Service name from docker-compose.yml
        private readonly string _queueName = "orderQueue"; // Queue name
        private readonly IInventoryService _inventoryService;

        public OrderEventConsumer(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            // Retry logic for connecting to RabbitMQ
            var connection = RetryUntilSuccess(() =>
            {
                return factory.CreateConnection();
            });

            using var channel = connection.CreateModel();

            // Declare the queue to consume from
            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                try
                {
                    var orderEvent = JsonConvert.DeserializeObject<OrderEvent>(message);
                    if (orderEvent != null)
                    {
                        Console.WriteLine($"Processing OrderEvent: OrderId = {orderEvent.Id}");

                        foreach (var orderLine in orderEvent.OrderLines)
                        {
                            _inventoryService.UpdateStock(orderLine.ISBN, orderLine.Quantity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            Console.WriteLine("Message handler is stopping..");
        }

        private T RetryUntilSuccess<T>(Func<T> action)
        {
            int retryCount = 0;
            const int maxRetries = 10;
            const int delay = 5000; // milliseconds

            while (retryCount < maxRetries)
            {
                try
                {
                    return action();
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException)
                {
                    retryCount++;
                    Console.WriteLine($"RabbitMQ is not reachable. Retry {retryCount}/{maxRetries}...");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Failed to connect to RabbitMQ after multiple retries.");
        }

    }
}
