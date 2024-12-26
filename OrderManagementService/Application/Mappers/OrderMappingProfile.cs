using AutoMapper;
using OrderManagementService.Application.DTOs;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Infrastructure.Messaging.Events;

namespace OrderManagementService.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Map between Order and its DTOs
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderLine, OrderLineDto>().ReverseMap();

            // Map Order to OrderEvent for messaging
            CreateMap<Order, CreatedOrderEvent>();
            CreateMap<OrderLine, OrderLineEvent>();
        }
    }

}
