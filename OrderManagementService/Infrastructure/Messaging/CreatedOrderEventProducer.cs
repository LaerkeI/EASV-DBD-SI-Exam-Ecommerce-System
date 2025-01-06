using Newtonsoft.Json;
using OrderManagementService.Infrastructure.Messaging.Events;
using RabbitMQ.Client;
using System.Text;

namespace OrderManagementService.Infrastructure.Messaging
{
    public class CreatedOrderEventProducer
    {
        private readonly string _hostName = "rabbitmq";  // RabbitMQ server host (service name in docker-compose.yaml)
        private readonly string _queueName = "createdOrderQueue"; // Queue name for order events

        public CreatedOrderEventProducer()
        {

        }

        public async Task PublishOrderEventAsync(CreatedOrderEvent orderEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,            // Matches RabbitMQ's AMQP port
                UserName = "guest",     
                Password = "guest"      
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