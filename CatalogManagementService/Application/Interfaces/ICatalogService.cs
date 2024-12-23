using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Application.Services
{
    public interface ICatalogService
    {
        Task<List<CatalogItem>> GetCatalogAsync();
        Task<List<CatalogItem>> GetCatalogItemsAsync();
        Task<CatalogItem?> GetCatalogItemByItemIdAsync(string itemId);
        Task CreateCatalogItemAsync(CatalogItem catalogItem);
        Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItem updatedCatalogItem);
        Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId);
        Task<bool> DeleteCatalogItemAsync(string itemId);
    }
}
