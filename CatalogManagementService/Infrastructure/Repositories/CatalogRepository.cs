using MongoDB.Driver;
using CatalogManagementService.Domain.Entities;
namespace CatalogManagementService.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoCollection<CatalogItem> _catalogItemsCollection;
        public CatalogRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("CatalogDB");
            _catalogItemsCollection = database.GetCollection<CatalogItem>("CatalogItems");
        }
        public async Task<List<CatalogItem>> GetCatalogAsync()
        {
            // Filter catalog items where IsAvailable is true and return as a list
            return await _catalogItemsCollection.Find(catalogItem => catalogItem.IsAvailable).ToListAsync();
        }
        public async Task<List<CatalogItem>> GetCatalogItemsAsync()
        {
            return await _catalogItemsCollection.Find(catalogItem => true).ToListAsync();
        }
        public async Task<CatalogItem?> GetCatalogItemByItemIdAsync(string itemId)
        {
            return await _catalogItemsCollection.Find(catalogItem => catalogItem.ItemId == itemId).FirstOrDefaultAsync();
        }
        public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
        {
            await _catalogItemsCollection.InsertOneAsync(catalogItem);
        }
        public async Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItem updatedCatalogItem)
        {
            // Create an update definition that includes all the fields to be updated
            var updateDefinition = Builders<CatalogItem>.Update
                .Set(c => c.Name, updatedCatalogItem.Name)
                .Set(c => c.Description, updatedCatalogItem.Description)
                .Set(c => c.Producer, updatedCatalogItem.Producer)
                .Set(c => c.Manufacturer, updatedCatalogItem.Manufacturer)
                .Set(c => c.IsAvailable, updatedCatalogItem.IsAvailable);
            // Perform the update
            var result = await _catalogItemsCollection.UpdateOneAsync(catalogItem => catalogItem.ItemId == itemId, updateDefinition);
            return result.MatchedCount > 0;
        }
        public async Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId)
        {
            // Create an update definition to set IsAvailable to false
            var updateDefinition = Builders<CatalogItem>.Update.Set(c => c.IsAvailable, false);
            // Perform the update
            var result = await _catalogItemsCollection.UpdateOneAsync(c => c.ItemId == itemId, updateDefinition);
            return result.MatchedCount > 0;
        }
        public async Task<bool> DeleteCatalogItemAsync(string itemId)
        {
            var result = await _catalogItemsCollection.DeleteOneAsync(catalogItem => catalogItem.ItemId == itemId);
            return result.DeletedCount > 0;
        }
    }
}