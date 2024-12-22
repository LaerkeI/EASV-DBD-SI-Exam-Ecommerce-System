using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Domain.Entities;
using InventoryManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementService.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryContext _context;

        public InventoryRepository(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync()
        {
            return await _context.InventoryItems
                .ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryItemByIdAsync(string id)
        {
            return await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<InventoryItem> AddInventoryItemAsync(InventoryItem inventoryItem)
        {
            await _context.InventoryItems.AddAsync(inventoryItem);
            await _context.SaveChangesAsync();
            return inventoryItem;
        }

        public async Task UpdateInventoryItemAsync(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Update(inventoryItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInventoryItemAsync(string id)
        {
            var inventoryItem = await GetInventoryItemByIdAsync(id);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
