using AutoMapper;
using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Domain.Entities;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Mappers
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile() 
        {
            // Map between InventoryItem and its DTOs
            CreateMap<InventoryItem, InventoryItemDto>().ReverseMap();

            // Map InventoryItem to OutOfStockEvent for messaging
            CreateMap<InventoryItem, OutOfStockEvent>();
        }
    }
}
