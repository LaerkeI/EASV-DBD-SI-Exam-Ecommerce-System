using AutoMapper;
using OrderManagementService.Entities;
using Shared.Contracts;

namespace OrderManagementService.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderEvent>();
        }
    }

}
