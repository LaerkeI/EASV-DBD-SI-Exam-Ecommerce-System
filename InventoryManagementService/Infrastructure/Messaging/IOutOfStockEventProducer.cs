using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Infrastructure.Messaging
{
    public interface IOutOfStockEventProducer
    {
        Task PublishOutOfStockEventAsync(OutOfStockEvent outOfStockEvent);
    }
}
