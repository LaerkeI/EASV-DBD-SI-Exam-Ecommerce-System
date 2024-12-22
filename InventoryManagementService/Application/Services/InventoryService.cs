using AutoMapper;
using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Domain.Entities;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        public readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IMapper mapper, IInventoryRepository inventoryRepository)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
        }
        
        public async Task<IEnumerable<InventoryItemDto>> GetInventoryItemsAsync()
        {
            var inventoryItems = await _inventoryRepository.GetInventoryItemsAsync();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(inventoryItems);
        }

        public async Task<InventoryItemDto> GetInventoryItemByIdAsync(string id)
        {
            var order = await _inventoryRepository.GetInventoryItemByIdAsync(id);
            return _mapper.Map<InventoryItemDto>(order);
        }

        public async Task<InventoryItemDto> CreateInventoryItemAsync(InventoryItemDto inventoryItemDto)
        {
            var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemDto);

            var createdInventoryItem = await _inventoryRepository.AddInventoryItemAsync(inventoryItem);

            //// Publish event after saving
            //var orderEvent = _mapper.Map<OrderEvent>(createdOrder);
            //await _orderEventProducer.PublishOrderEventAsync(orderEvent);

            return _mapper.Map<InventoryItemDto>(createdInventoryItem);
        }

        public async Task UpdateInventoryItemAsync(InventoryItemDto inventoryItemDto)
        {
            Console.WriteLine($"Updating stock for book = {inventoryItemDto.Id}. Stock lowered by {inventoryItemDto.Quantity}");
            var existingInventoryItem = await _inventoryRepository.GetInventoryItemByIdAsync(inventoryItemDto.Id);
            if (existingInventoryItem != null)
            {
                _mapper.Map(inventoryItemDto, existingInventoryItem);
                await _inventoryRepository.UpdateInventoryItemAsync(existingInventoryItem);
            }
        }

        public async Task DeleteOrderAsync(string id)
        {
            await _inventoryRepository.DeleteInventoryItemAsync(id);
        }
        public void UpdateStock(string id, int quantity)
        {
            // Implement the logic to update the stock based on the orderEvent.
            Console.WriteLine($"Updating stock for book = {id}. Stock lowered by {quantity}");
            // Example: Decrease stock quantities for ordered items.
        }
    }
}
