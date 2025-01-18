using MongoDB.Driver;
using CatalogManagementService.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using CatalogManagementService.Infrastructure.Utils;

namespace CatalogManagementService.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoCollection<CatalogItem> _catalogItemsCollection;
        private readonly IDistributedCache _redisCache; // Inject Redis cache

        public CatalogRepository(IMongoClient mongoClient, IDistributedCache redisCache)
        {
            var database = mongoClient.GetDatabase("CatalogDB");
            _catalogItemsCollection = database.GetCollection<CatalogItem>("CatalogItems");
            _redisCache = redisCache;
        }

        public async Task<List<CatalogItem>> GetCatalogAsync()
        {
            const string cacheKey = "CatalogItems:Available";

            // Check cache first
            var cachedCatalog = await _redisCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedCatalog))
            {
                Console.WriteLine($"Cache hit for key: {cacheKey}");
                return JsonConvert.DeserializeObject<List<CatalogItem>>(cachedCatalog, JsonSettingsProvider.GetJsonSerializerSettings())!;
            }

            // If not in cache, fetch from database
            var catalogItems = await _catalogItemsCollection.Find(catalogItem => catalogItem.IsAvailable).ToListAsync();

            // Cache the result
            await _redisCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(catalogItems, JsonSettingsProvider.GetJsonSerializerSettings()), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            Console.WriteLine($"Cache set for key: {cacheKey}");
            return catalogItems;
        }

        public async Task<List<CatalogItem>> GetCatalogItemsAsync()
        {
            const string cacheKey = "CatalogItems:All";

            // Check cache first
            var cachedCatalogItems = await _redisCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedCatalogItems))
            {
                Console.WriteLine($"Cache hit for key: {cacheKey}");
                return JsonConvert.DeserializeObject<List<CatalogItem>>(cachedCatalogItems, JsonSettingsProvider.GetJsonSerializerSettings())!;
            }

            // If not in cache, fetch from database
            var catalogItems = await _catalogItemsCollection.Find(catalogItem => true).ToListAsync();

            // Cache the result
            await _redisCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(catalogItems, JsonSettingsProvider.GetJsonSerializerSettings()), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            Console.WriteLine($"Cache set for key: {cacheKey}");
            return catalogItems;
        }

        public async Task<CatalogItem?> GetCatalogItemByItemIdAsync(string itemId)
        {
            var cacheKey = $"CatalogItems:{itemId}";

            // Check cache first
            var cachedItem = await _redisCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedItem))
            {
                Console.WriteLine($"Cache hit for key: {cacheKey}");
                return JsonConvert.DeserializeObject<CatalogItem>(cachedItem, JsonSettingsProvider.GetJsonSerializerSettings());
            }

            // If not in cache, fetch from database
            var catalogItem = await _catalogItemsCollection.Find(catalogItem => catalogItem.ItemId == itemId).FirstOrDefaultAsync();

            if (catalogItem != null)
            {
                // Cache the result
                await _redisCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(catalogItem, JsonSettingsProvider.GetJsonSerializerSettings()), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

                Console.WriteLine($"Cache set for key: {cacheKey}");
            }

            return catalogItem;
        }

        public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
        {
            // Check if an item with the same ItemId already exists
            var existingItem = await _catalogItemsCollection
                .Find(item => item.ItemId == catalogItem.ItemId)
                .FirstOrDefaultAsync();

            if (existingItem != null)
            {
                throw new InvalidOperationException($"A CatalogItem with ItemId '{catalogItem.ItemId}' already exists.");
            }

            // Insert the new CatalogItem
            await _catalogItemsCollection.InsertOneAsync(catalogItem);

            // Invalidate cache for catalog lists
            await _redisCache.RemoveAsync("CatalogItems:All");
            await _redisCache.RemoveAsync("CatalogItems:Available");

            Console.WriteLine("Cache invalidated for keys: CatalogItems:All, CatalogItems:Available");
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

            if (result.MatchedCount > 0)
            {
                // Invalidate cache for this item and catalog lists
                await _redisCache.RemoveAsync($"CatalogItems:{itemId}");
                await _redisCache.RemoveAsync("CatalogItems:All");
                await _redisCache.RemoveAsync("CatalogItems:Available");

                Console.WriteLine($"Cache invalidated for keys: CatalogItems:{itemId}, CatalogItems:All, CatalogItems:Available");
            }

            return result.MatchedCount > 0;
        }

        public async Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId)
        {
            // Create an update definition to set IsAvailable to false
            var updateDefinition = Builders<CatalogItem>.Update.Set(c => c.IsAvailable, false);

            // Perform the update
            var result = await _catalogItemsCollection.UpdateOneAsync(c => c.ItemId == itemId, updateDefinition);

            if (result.MatchedCount > 0)
            {
                // Invalidate cache for this item and catalog lists
                await _redisCache.RemoveAsync($"CatalogItems:{itemId}");
                await _redisCache.RemoveAsync("CatalogItems:Available");

                Console.WriteLine($"Cache invalidated for keys: CatalogItems:{itemId}, CatalogItems:Available");
            }

            return result.MatchedCount > 0;
        }

        public async Task<bool> DeleteCatalogItemAsync(string itemId)
        {
            var result = await _catalogItemsCollection.DeleteOneAsync(catalogItem => catalogItem.ItemId == itemId);

            if (result.DeletedCount > 0)
            {
                // Invalidate cache for this item and catalog lists
                await _redisCache.RemoveAsync($"CatalogItems:{itemId}");
                await _redisCache.RemoveAsync("CatalogItems:All");
                await _redisCache.RemoveAsync("CatalogItems:Available");

                Console.WriteLine($"Cache invalidated for keys: CatalogItems:{itemId}, CatalogItems:All, CatalogItems:Available");
            }

            return result.DeletedCount > 0;
        }
    }
}