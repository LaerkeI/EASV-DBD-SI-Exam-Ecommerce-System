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
            // Read-only query; no changes needed for ACID.
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderLines)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByOrderIdAsync(int orderId)
        {
            // Read-only query; no changes needed for ACID.
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

                if (existingOrder == null)
                    throw new InvalidOperationException($"Order with ID {order.OrderId} not found.");

                // Update scalar properties of the Order (excluding OrderId)
                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                // Sync OrderLines
                // 1. Remove lines that are no longer present
                var linesToRemove = existingOrder.OrderLines
                    .Where(existingLine => !order.OrderLines.Any(updatedLine =>
                        updatedLine.OrderId == existingLine.OrderId &&
                        updatedLine.ItemId == existingLine.ItemId))
                    .ToList();

                foreach (var lineToRemove in linesToRemove)
                {
                    _context.OrderLines.Remove(lineToRemove);
                }

                // 2. Add or update lines
                foreach (var updatedLine in order.OrderLines)
                {
                    var existingLine = existingOrder.OrderLines
                        .FirstOrDefault(existing =>
                            existing.OrderId == updatedLine.OrderId &&
                            existing.ItemId == updatedLine.ItemId);

                    if (existingLine != null)
                    {
                        // Update existing line (keys remain unchanged)
                        _context.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                    }
                    else
                    {
                        // Add new line
                        existingOrder.OrderLines.Add(updatedLine);
                    }
                }

                // Save changes
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new InvalidOperationException($"Order with ID {orderId} not found.");

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                // Rollback transaction on failure
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
