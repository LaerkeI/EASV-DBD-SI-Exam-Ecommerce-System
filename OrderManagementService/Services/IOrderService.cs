using Shared.Contracts;

namespace OrderManagementService.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderEvent orderEvent);
    }
}
