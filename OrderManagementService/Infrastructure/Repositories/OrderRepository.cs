using OrderManagementService.Application.Interfaces;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await GetOrderByOrderIdAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}

