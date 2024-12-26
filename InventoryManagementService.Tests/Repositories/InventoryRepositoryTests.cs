using InventoryManagementService.Infrastructure.Data;
using InventoryManagementService.Infrastructure.Repositories;
using InventoryManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InventoryManagementService.Tests.Repositories
{
    public class InventoryRepositoryTests
    {
        private InventoryContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<InventoryContext>()
                .UseInMemoryDatabase(databaseName: "TestInventoryDB")
                .Options;
            return new InventoryContext(options);
        }

        [Fact]
        public async Task GetInventoryItemsAsync_ShouldReturnAllItems()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.InventoryItems.AddRange(
                new InventoryItem { ItemId = "1", Quantity = 10 },
                new InventoryItem { ItemId = "2", Quantity = 5 }
            );
            await context.SaveChangesAsync();

            var repository = new InventoryRepository(context);

            // Act
            var items = await repository.GetInventoryItemsAsync();

            // Assert
            Assert.NotNull(items);
            Assert.Equal(2, items.Count());
            Assert.Contains(items, i => i.ItemId == "1" && i.Quantity == 10);
            Assert.Contains(items, i => i.ItemId == "2" && i.Quantity == 5);
        }
    }
}
