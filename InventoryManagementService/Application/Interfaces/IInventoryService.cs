using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItemDto>> GetInventoryItemsAsync();
        Task<InventoryItemDto> GetInventoryItemByItemIdAsync(string itemId);
        Task<InventoryItemDto> CreateInventoryItemAsync(InventoryItemDto inventoryItemDto);
        Task UpdateInventoryItemAsync(InventoryItemDto inventoryItemDto);
        Task ReduceQuantityForInventoryItemAsync(InventoryItemDto inventoryItemDto);
        Task DeleteInventoryItemAsync(string itemId);
    }

}
