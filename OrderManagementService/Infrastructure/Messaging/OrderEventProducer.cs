using Newtonsoft.Json;
using OrderManagementService.Infrastructure.Messaging.Contracts;
using RabbitMQ.Client;
using System.Text;

namespace OrderManagementService.Infrastructure.Messaging
{
    public class OrderEventProducer
    {
        private readonly string _hostName = "rabbitmq";  // RabbitMQ server host (service name in docker-compose.yml)
        private readonly string _queueName = "orderQueue"; // Queue name for order events

        public OrderEventProducer()
        {
            // No dependency injection needed for RabbitMQ client in this example
        }

        public async Task PublishOrderEventAsync(OrderEvent orderEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,            // Ensure this matches RabbitMQ's AMQP port
                UserName = "guest",     // Replace if using non-default credentials
                Password = "guest"      // Replace if using non-default credentials
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Ensure the queue exists (it will be created if it doesn't)
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