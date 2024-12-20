using AutoMapper;
using OrderManagementService.Infrastructure.Messaging;
using OrderManagementService.Application.Interfaces;
using OrderManagementService.Application.DTOs;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Infrastructure.Messaging.Events;

namespace OrderManagementService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderEventProducer _orderEventProducer;

        public OrderService(IMapper mapper, IOrderRepository orderRepository, OrderEventProducer orderEventProducer)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _orderEventProducer = orderEventProducer;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);

            var createdOrder = await _orderRepository.AddOrderAsync(order);

            // Publish event after saving
            var orderEvent = _mapper.Map<OrderEvent>(createdOrder);
            await _orderEventProducer.PublishOrderEventAsync(orderEvent);

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task UpdateOrderAsync(OrderDto orderDto) // But would this method even be realistical? It is generally not possible to update an order
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderDto.Id);
            if (existingOrder != null)
            {
                _mapper.Map(orderDto, existingOrder);
                await _orderRepository.UpdateOrderAsync(existingOrder);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}