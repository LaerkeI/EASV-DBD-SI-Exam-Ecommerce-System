using OrderManagementService.Application.DTOs;

namespace OrderManagementService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetOrderAsync(int id);
        Task<OrderDto> CreateOrderAsync(OrderDto orderDto);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int id);
    }
}
