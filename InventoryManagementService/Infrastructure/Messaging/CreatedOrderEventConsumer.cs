﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using InventoryManagementService.Infrastructure.Messaging.Events;
using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Application.DTOs;

namespace InventoryManagementService.Infrastructure.Messaging
{
    public class CreatedOrderEventConsumer : BackgroundService
    {
        private readonly string _hostName = "rabbitmq"; // RabbitMQ server host. Service name from docker-compose.yaml
        private readonly string _queueName = "createdOrderQueue"; // Queue name
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CreatedOrderEventConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
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
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                try
                {
                    var orderEvent = JsonConvert.DeserializeObject<CreatedOrderEvent>(message);
                    if (orderEvent != null)
                    {
                        Console.WriteLine($"Processing CreatedOrderEvent: OrderId = {orderEvent.OrderId}");

                        foreach (var orderLine in orderEvent.OrderLines)
                        {
                            // Map orderLine to InventoryItemDto
                            var inventoryItemDto = new InventoryItemDto
                            {
                                ItemId = orderLine.ItemId,
                                Quantity = orderLine.Quantity
                            };

                            // Resolve IInventoryService from IServiceProvider and call the service to update the inventory
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();
                                await inventoryService.ReduceQuantityForInventoryItemAsync(inventoryItemDto);
                            }
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
