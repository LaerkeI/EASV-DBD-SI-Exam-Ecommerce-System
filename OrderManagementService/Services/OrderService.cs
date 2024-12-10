using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Contracts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Services
{
    public class OrderService : IOrderService
    {
        private readonly string _hostName = "rabbitmq";  // RabbitMQ server host. Service name from docker-compose.yml
        private readonly string _queueName = "orderQueue"; // Queue name for order events

        public OrderService()
        {
            // No dependency injection for RabbitMQ client in this example
        }

        public async Task CreateOrderAsync(OrderEvent orderEvent)
        {
            // Business logic for creating an order
            Console.WriteLine("Order created: " + orderEvent.Id);

            // Publish the order event to RabbitMQ
            await PublishOrderEventAsync(orderEvent);
            Console.WriteLine("Order event published.");
        }

        private async Task PublishOrderEventAsync(OrderEvent orderEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName, 
                Port = 5672,            // Ensure this matches your RabbitMQ's AMQP port
                UserName = "guest",     // Replace if using non-default credentials
                Password = "guest"      // Replace if using non-default credentials
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Ensure the queue exists (if not, it will be created)
            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Serialize the OrderEvent to JSON
            var message = JsonConvert.SerializeObject(orderEvent);
            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message to the queue
            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);

            await Task.CompletedTask;
        }
    }
}
