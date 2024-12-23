using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Infrastructure.Repositories;

namespace CatalogManagementService.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task<List<CatalogItem>> GetCatalogAsync()
        {
            return await _catalogRepository.GetCatalogAsync();
        }

        public async Task<List<CatalogItem>> GetCatalogItemsAsync()
        {
            return await _catalogRepository.GetCatalogItemsAsync();
        }

        public async Task<CatalogItem?> GetCatalogItemByItemIdAsync(string itemId)
        {
            return await _catalogRepository.GetCatalogItemByItemIdAsync(itemId);
        }

        public async Task CreateCatalogItemAsync(CatalogItem catalogItem)
        {
            await _catalogRepository.CreateCatalogItemAsync(catalogItem);
        }

        public async Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItem updatedCatalogItem)
        {
            return await _catalogRepository.UpdateCatalogItemAsync(itemId, updatedCatalogItem);
        }

        public async Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId)
        {
            return await _catalogRepository.UpdateAvailabilityOfCatalogItemAsync(itemId);
        }

        public async Task<bool> DeleteCatalogItemAsync(string itemId)
        {
            return await _catalogRepository.DeleteCatalogItemAsync(itemId);
        }
    }
}
