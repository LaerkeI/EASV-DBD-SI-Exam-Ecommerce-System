using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Shared.Contracts;
using System;
using System.Text;

namespace InventoryManagementService.Services
{
    public class OrderEventConsumer
    {
        private readonly string _hostName = "rabbitmq"; // RabbitMQ server host. Service name from docker-compose.yml
        private readonly string _queueName = "orderQueue"; // Queue name

        public void StartConsuming()
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
                    Console.WriteLine($"Received Order Event: Id = {orderEvent?.Id}, Date = {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Consumer started. Press [enter] to exit.");
            Console.ReadLine();
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
