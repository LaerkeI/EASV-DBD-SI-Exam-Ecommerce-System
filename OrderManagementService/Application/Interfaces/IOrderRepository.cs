using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByOrderIdAsync(int orderId);
        Task<Order> AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
    }
}
