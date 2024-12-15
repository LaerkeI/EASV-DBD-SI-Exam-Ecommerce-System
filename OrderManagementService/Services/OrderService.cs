using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Data;
using OrderManagementService.Messaging;
using OrderManagementService.Entities;
using Shared.Contracts;
using Shared.DTOs;

namespace OrderManagementService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly OrderContext _context;
        private readonly OrderEventProducer _orderEventProducer;

        public OrderService(IMapper mapper, OrderContext context, OrderEventProducer orderEventProducer)
        {
            _mapper = mapper;
            _context = context;
            _orderEventProducer = orderEventProducer;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            // Map the DTO to the Order entity
            var order = _mapper.Map<Order>(orderDto);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Map the Order entity to OrderEvent for messaging
            var orderEvent = _mapper.Map<OrderEvent>(order);

            await _orderEventProducer.PublishOrderEventAsync(orderEvent);

            // Map the saved Order entity back to OrderDto
            return _mapper.Map<OrderDto>(order);
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
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
