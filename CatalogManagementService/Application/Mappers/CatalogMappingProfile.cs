using AutoMapper;
using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Application.Mappers
{
    public class CatalogMappingProfile : Profile
    {
        public CatalogMappingProfile()
        {
            // Map between InventoryItem and its DTOs
            CreateMap<CatalogItem, CatalogItemDto>().ReverseMap();
        }
    }
}
