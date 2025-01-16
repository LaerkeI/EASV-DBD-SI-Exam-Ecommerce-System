using MongoDB.Driver;
using CatalogManagementService.Domain.Entities;
using MongoDB.Bson;

namespace CatalogManagementService.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoCollection<CatalogItem> _catalogItemsCollection;
        private readonly IMongoClient _mongoClient;

        public CatalogRepository(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            var database = mongoClient.GetDatabase("CatalogDB");
            _catalogItemsCollection = database.GetCollection<CatalogItem>("CatalogItems");
        }

        public async Task<List<CatalogItem>> GetCatalogAsync()
        {
            // Read-only query; no need for ACID transaction.
            return await _catalogItemsCollection.Find(catalogItem => catalogItem.IsAvailable).ToListAsync();
        }

        public async Task<List<CatalogItem>> GetCatalogItemsAsync()
        {
            // Read-only query; no need for ACID transaction.
            return await _catalogItemsCollection.Find(catalogItem => true).ToListAsync();
        }

        public async Task<CatalogItem?> GetCatalogItemByItemIdAsync(string itemId)
        {
            // Read-only query; no need for ACID transaction.
            return await _catalogItemsCollection.Find(catalogItem => catalogItem.ItemId == itemId).FirstOrDefaultAsync();
        }

        public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
        {
            if (catalogItem == null)
                throw new ArgumentNullException(nameof(catalogItem));

            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();  // Begin the transaction

            try
            {
                // Insert the catalog item within the transaction
                await _catalogItemsCollection.InsertOneAsync(session, catalogItem);
                await session.CommitTransactionAsync();  // Commit the transaction

            }
            catch
            {
                await session.AbortTransactionAsync();  // Rollback on failure
                throw;
            }
        }

        public async Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItem updatedCatalogItem)
        {
            if (updatedCatalogItem == null)
                throw new ArgumentNullException(nameof(updatedCatalogItem));

            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();  // Begin the transaction

            try
            {
                // Create an update definition
                var updateDefinition = Builders<CatalogItem>.Update
                    .Set(c => c.Name, updatedCatalogItem.Name)
                    .Set(c => c.Description, updatedCatalogItem.Description)
                    .Set(c => c.Producer, updatedCatalogItem.Producer)
                    .Set(c => c.Manufacturer, updatedCatalogItem.Manufacturer)
                    .Set(c => c.IsAvailable, updatedCatalogItem.IsAvailable);

                // Perform the update within the transaction
                var result = await _catalogItemsCollection.UpdateOneAsync(session, catalogItem => catalogItem.ItemId == itemId, updateDefinition);

                // If any document was matched and modified, commit the transaction
                if (result.MatchedCount > 0)
                {
                    await session.CommitTransactionAsync();
                    return true;
                }

                await session.AbortTransactionAsync();  // Rollback if no match found
                return false;
            }
            catch
            {
                await session.AbortTransactionAsync();  // Rollback on failure
                throw;
            }
        }

        public async Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId)
        {
            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();  // Begin the transaction

            try
            {
                // Create an update definition to set IsAvailable to false
                var updateDefinition = Builders<CatalogItem>.Update.Set(c => c.IsAvailable, false);

                // Perform the update within the transaction
                var result = await _catalogItemsCollection.UpdateOneAsync(session, c => c.ItemId == itemId, updateDefinition);

                // If any document was matched, commit the transaction
                if (result.MatchedCount > 0)
                {
                    await session.CommitTransactionAsync();
                    return true;
                }

                await session.AbortTransactionAsync();  // Rollback if no match found
                return false;
            }
            catch
            {
                await session.AbortTransactionAsync();  // Rollback on failure
                throw;
            }
        }

        public async Task<bool> DeleteCatalogItemAsync(string itemId)
        {
            using var session = await _mongoClient.StartSessionAsync();
            session.StartTransaction();  // Begin the transaction

            try
            {
                // Perform the deletion within the transaction
                var result = await _catalogItemsCollection.DeleteOneAsync(session, catalogItem => catalogItem.ItemId == itemId);

                // If any document was deleted, commit the transaction
                if (result.DeletedCount > 0)
                {
                    await session.CommitTransactionAsync();
                    return true;
                }

                await session.AbortTransactionAsync();  // Rollback if nothing was deleted
                return false;
            }
            catch
            {
                await session.AbortTransactionAsync();  // Rollback on failure
                throw;
            }
        }
    }
}
