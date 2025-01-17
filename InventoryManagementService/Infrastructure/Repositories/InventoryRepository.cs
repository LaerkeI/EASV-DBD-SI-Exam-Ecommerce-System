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
            // Read-only query; no changes needed for ACID.
            return await _context.InventoryItems
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryItemByItemIdAsync(string itemId)
        {
            // Read-only query; no changes needed for ACID.
            return await _context.InventoryItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<InventoryItem> AddInventoryItemAsync(InventoryItem inventoryItem)
        {
            if (inventoryItem == null)
                throw new ArgumentNullException(nameof(inventoryItem));

            // Check if an InventoryItem with the same ItemId already exists
            var existingItem = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.ItemId == inventoryItem.ItemId);

            if (existingItem != null)
                throw new InvalidOperationException($"Inventory item with ID {inventoryItem.ItemId} already exists.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the new InventoryItem
                await _context.InventoryItems.AddAsync(inventoryItem);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
                return inventoryItem;
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateInventoryItemAsync(InventoryItem inventoryItem)
        {
            if (inventoryItem == null)
                throw new ArgumentNullException(nameof(inventoryItem));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if the inventory item exists
                var existingInventoryItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(i => i.ItemId == inventoryItem.ItemId);

                if (existingInventoryItem == null)
                    throw new InvalidOperationException($"Inventory item with ItemId {inventoryItem.ItemId} not found.");

                // Update scalar properties of the InventoryItem
                _context.Entry(existingInventoryItem).CurrentValues.SetValues(inventoryItem);

                // Save changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteInventoryItemAsync(string itemId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var inventoryItem = await GetInventoryItemByItemIdAsync(itemId);
                if (inventoryItem == null)
                    throw new InvalidOperationException($"Inventory item with ItemId {itemId} not found.");

                _context.InventoryItems.Remove(inventoryItem);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
