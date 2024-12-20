using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Interfaces
{
    public interface IInventoryService
    {
        void UpdateStock(string id, int quantity);
    }

}
