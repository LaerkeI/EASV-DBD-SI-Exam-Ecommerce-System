using AutoMapper;
using OrderManagementService.Entities;
using Shared.Contracts;
using Shared.DTOs;

namespace OrderManagementService.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Map between Order and its DTOs
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderLine, OrderLineDto>().ReverseMap();

            // Map Order to OrderEvent for messaging
            CreateMap<Order, OrderEvent>();
        }
    }

}
