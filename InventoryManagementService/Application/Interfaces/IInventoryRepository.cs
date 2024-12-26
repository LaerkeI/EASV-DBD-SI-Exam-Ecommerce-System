using InventoryManagementService.Domain.Entities;

namespace InventoryManagementService.Application.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync();
        Task<InventoryItem> GetInventoryItemByItemIdAsync(string itemid);
        Task<InventoryItem> AddInventoryItemAsync(InventoryItem inventoryItem);
        Task UpdateInventoryItemAsync(InventoryItem inventoryItem);
        Task DeleteInventoryItemAsync(string itemId);

    }
}
