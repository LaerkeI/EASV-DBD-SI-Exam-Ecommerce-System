using MongoDB.Driver;
using Moq;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Infrastructure.Repositories;
using Xunit;
using System.Threading.Tasks;

namespace CatalogManagementService.Tests.Repositories
{
    public class CatalogRepositoryTests
    {
        [Fact]
        public async Task CreateCatalogItemAsync_ShouldInsertItem()
        {
            // Arrange
            var catalogItem = new CatalogItem
            {
                ItemId = "1",
                Name = "Item1",
                Description = "Description1",
                Producer = "Producer1",
                Manufacturer = "Manufacturer1",
                IsAvailable = true
            };

            // Mock the IMongoCollection to simulate InsertOneAsync
            var mockCollection = new Mock<IMongoCollection<CatalogItem>>();
            mockCollection.Setup(c => c.InsertOneAsync(It.IsAny<CatalogItem>(), null, default))
                          .Returns(Task.CompletedTask);  // Simulate that InsertOneAsync is successful

            // Mock the MongoDatabase and MongoClient
            var mockDb = new Mock<IMongoDatabase>();
            mockDb.Setup(db => db.GetCollection<CatalogItem>("CatalogItems", null))
                  .Returns(mockCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                      .Returns(mockDb.Object);

            var repository = new CatalogRepository(mockClient.Object);  // Create the repository with the mocked client

            // Act
            await repository.CreateCatalogItemAsync(catalogItem);  // Call the method to insert the catalog item

            // Assert
            mockCollection.Verify(c => c.InsertOneAsync(It.Is<CatalogItem>(item => item.ItemId == catalogItem.ItemId
                                                                               && item.Name == catalogItem.Name
                                                                               && item.Description == catalogItem.Description
                                                                               && item.Producer == catalogItem.Producer
                                                                               && item.Manufacturer == catalogItem.Manufacturer
                                                                               && item.IsAvailable == catalogItem.IsAvailable),
                                                      null, default),
                           Times.Once);  // Ensure InsertOneAsync was called exactly once with the correct catalog item
        }
    }
}