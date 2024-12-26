using AutoMapper;
using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Application.Interfaces;
using InventoryManagementService.Domain.Entities;
using InventoryManagementService.Infrastructure.Messaging;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly OutOfStockEventProducer _outOfStockEventProducer;

        public InventoryService(IMapper mapper, IInventoryRepository inventoryRepository, OutOfStockEventProducer outOfStockEventProducer)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _outOfStockEventProducer = outOfStockEventProducer;
        }
        
        public async Task<IEnumerable<InventoryItemDto>> GetInventoryItemsAsync()
        {
            var inventoryItems = await _inventoryRepository.GetInventoryItemsAsync();
            return _mapper.Map<IEnumerable<InventoryItemDto>>(inventoryItems);
        }

        public async Task<InventoryItemDto> GetInventoryItemByItemIdAsync(string itemId)
        {
            var order = await _inventoryRepository.GetInventoryItemByItemIdAsync(itemId);
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

        public async Task UpdateInventoryItemAsync(InventoryItemDto inventoryItemDto) //For restocking InventoryItem
        {
            Console.WriteLine($"Updating stock for book = {inventoryItemDto.ItemId}. Stock lowered by {inventoryItemDto.Quantity}");
            var existingInventoryItem = await _inventoryRepository.GetInventoryItemByItemIdAsync(inventoryItemDto.ItemId);
            if (existingInventoryItem != null)
            {
                _mapper.Map(inventoryItemDto, existingInventoryItem);
                await _inventoryRepository.UpdateInventoryItemAsync(existingInventoryItem);
            }
        }

        public async Task ReduceQuantityForInventoryItemAsync(InventoryItemDto inventoryItemDto)
        {
            Console.WriteLine($"Attempting to update stock for item = {inventoryItemDto.ItemId}. Stock lowered by {inventoryItemDto.Quantity}");

            var existingInventoryItem = await _inventoryRepository.GetInventoryItemByItemIdAsync(inventoryItemDto.ItemId);

            if (existingInventoryItem == null)
            {
                throw new Exception($"Inventory item with ItemId {inventoryItemDto.ItemId} not found.");
            }

            // Check if the quantity requested is available in stock
            if (existingInventoryItem.Quantity < inventoryItemDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available stock for item {inventoryItemDto.ItemId} is {existingInventoryItem.Quantity}. Requested quantity is {inventoryItemDto.Quantity}.");
            }

            existingInventoryItem.Quantity -= inventoryItemDto.Quantity;

            if (existingInventoryItem.Quantity == 0)
            {
                // Send message to CatalogManagementService to remove it from display list.
                Console.WriteLine("Sending message to CatalogManagementService ...");
                // Publish event after saving
                var outOfStockEvent = _mapper.Map<OutOfStockEvent>(existingInventoryItem);
                await _outOfStockEventProducer.PublishOutOfStockEventAsync(outOfStockEvent);
            }

            await _inventoryRepository.UpdateInventoryItemAsync(existingInventoryItem);
        }


        public async Task DeleteInventoryItemAsync(string itemId)
        {
            await _inventoryRepository.DeleteInventoryItemAsync(itemId);
        }
    }
}
