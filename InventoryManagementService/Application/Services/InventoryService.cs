using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Services
{
    public class InventoryService : IInventoryService
    {
        public void UpdateStock(string isbn, int quantity)
        {
            // Implement the logic to update the stock based on the orderEvent.
            Console.WriteLine($"Updating stock for book = {isbn}. Stock lowered by {quantity}");
            // Example: Decrease stock quantities for ordered items.
        }
    }
}
