using AutoMapper;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Domain.Entities;
using CatalogManagementService.Infrastructure.Repositories;

namespace CatalogManagementService.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IMapper _mapper;

        public CatalogService(ICatalogRepository catalogRepository, IMapper mapper)
        {
            _catalogRepository = catalogRepository;
            _mapper = mapper;
        }

        public async Task<List<CatalogItemDto>> GetCatalogAsync()
        {
            var catalog = await _catalogRepository.GetCatalogAsync();
            return _mapper.Map<List<CatalogItemDto>>(catalog);
        }

        public async Task<List<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var catalogItems = await _catalogRepository.GetCatalogItemsAsync();
            return _mapper.Map<List<CatalogItemDto>>(catalogItems);
        }

        public async Task<CatalogItemDto?> GetCatalogItemByItemIdAsync(string itemId)
        {
            var catalogItem = await _catalogRepository.GetCatalogItemByItemIdAsync(itemId);
            return catalogItem == null ? null : _mapper.Map<CatalogItemDto>(catalogItem);
        }

        public async Task CreateCatalogItemAsync(CatalogItemDto catalogItemDto)
        {
            var catalogItem = _mapper.Map<CatalogItem>(catalogItemDto);
            await _catalogRepository.CreateCatalogItemAsync(catalogItem);
        }

        public async Task<bool> UpdateCatalogItemAsync(string itemId, CatalogItemDto updatedCatalogItemDto)
        {
            var catalogItem = _mapper.Map<CatalogItem>(updatedCatalogItemDto);
            return await _catalogRepository.UpdateCatalogItemAsync(itemId, catalogItem);
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
