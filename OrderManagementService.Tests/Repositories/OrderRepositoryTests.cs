using OrderManagementService.Infrastructure.Data;
using OrderManagementService.Infrastructure.Repositories;
using OrderManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace OrderManagementService.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private OrderContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: "TestOrderDB")
                .Options;
            return new OrderContext(options);
        }

        [Fact]
        public async Task GetOrdersAsync_ShouldReturnAllOrders()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.Orders.AddRange(
                new Order
                {
                    Id = 1,
                    OrderDate = DateTime.Now,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine { ItemId = "I1", Quantity = 2 }
                    }
                },
                new Order
                {
                    Id = 2,
                    OrderDate = DateTime.Now,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine { ItemId = "I2", Quantity = 1 }
                    }
                }
            );
            await context.SaveChangesAsync();

            var repository = new OrderRepository(context);

            // Act
            var orders = await repository.GetOrdersAsync();

            // Assert
            Assert.NotNull(orders);
            Assert.Equal(2, orders.Count());
            Assert.Contains(orders, o => o.Id == 1 && o.OrderLines.Any(ol => ol.ItemId == "I1" && ol.Quantity == 2));
            Assert.Contains(orders, o => o.Id == 2 && o.OrderLines.Any(ol => ol.ItemId == "I2" && ol.Quantity == 1));
        }
    }
}
