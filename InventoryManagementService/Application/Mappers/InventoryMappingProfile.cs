using AutoMapper;
using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Domain.Entities;

namespace InventoryManagementService.Application.Mappers
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile() 
        {
            // Map between InventoryItem and its DTOs
            CreateMap<InventoryItem, InventoryItemDto>().ReverseMap();

            // Map Order to OrderEvent for messaging
            //CreateMap<Order, OrderEvent>();
            //CreateMap<OrderLine, OrderLineEvent>();
        }
    }
}
