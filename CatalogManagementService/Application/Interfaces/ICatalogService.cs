using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.Application.Services
{
    public interface ICatalogService
    {
        Task<List<CatalogItemDto>> GetCatalogAsync();
        Task<List<CatalogItemDto>> GetCatalogItemsAsync();
        Task<CatalogItemDto?> GetCatalogItemByItemIdAsync(string itemId);
        Task CreateCatalogItemAsync(CatalogItemDto catalogItem);
        Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItemDto updatedCatalogItem);
        Task<bool> UpdateAvailabilityOfCatalogItemAsync(string itemId);
        Task<bool> DeleteCatalogItemAsync(string itemId);
    }
}
