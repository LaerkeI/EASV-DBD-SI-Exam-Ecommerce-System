using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Data;
using OrderManagementService.Messaging;
using OrderManagementService.Entities;
using Shared.Contracts;


namespace OrderManagementService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly OrderContext _context;
        private readonly OrderEventProducer _orderEventProducer;

        public OrderService(IMapper _mapper, OrderContext context, OrderEventProducer orderEventProducer)
        {
            _context = context;
            _orderEventProducer = orderEventProducer;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderEvent = new OrderEvent
            {
                Id = order.Id,
                BookISBN = order.BookISBN
            };

            // Publish the OrderEvent to the message queue
            await _orderEventProducer.PublishOrderEventAsync(orderEvent);

            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
