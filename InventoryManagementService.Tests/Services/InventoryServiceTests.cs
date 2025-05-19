using Xunit;
using Moq;
using InventoryManagementService.Application.Interfaces;
using AutoMapper;
using InventoryManagementService.Infrastructure.Messaging;
using InventoryManagementService.Application.Services;
using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Domain.Entities;
using InventoryManagementService.Infrastructure.Messaging.Events;

namespace InventoryManagementService.Tests.Repositories
{
    public class InventoryServiceTests
    {
        private readonly Mock<IInventoryRepository> _inventoryRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOutOfStockEventProducer> _eventProducerMock = new();
        private readonly InventoryService _service;

        public InventoryServiceTests()
        {
            _service = new InventoryService(
                _mapperMock.Object,
                _inventoryRepositoryMock.Object,
                _eventProducerMock.Object
            );
        }

        public static IEnumerable<object[]> GetTestCases()
        {
            yield return new object[] {
                "TC1 - Item not found",
                new InventoryItemDto { ItemId = "A1", Quantity = 1 },
                null, // inventory item not found
                typeof(Exception),
                false, false, null
            };

            yield return new object[] {
                "TC2 - Insufficient quantity",
                new InventoryItemDto { ItemId = "B2", Quantity = 10 },
                new InventoryItem { ItemId = "B2", Quantity = 5 },
                typeof(InvalidOperationException),
                false, false, null
            };

            yield return new object[] {
                "TC3 - Valid update, out of stock event",
                new InventoryItemDto { ItemId = "D4", Quantity = 10 },
                new InventoryItem { ItemId = "D4", Quantity = 10 },
                null,
                true, true, 0 // expect update and event, final qty = 0
            };

            yield return new object[] {
                "TC4 - Valid update, stock remains",
                new InventoryItemDto { ItemId = "C3", Quantity = 3 },
                new InventoryItem { ItemId = "C3", Quantity = 5 },
                null,
                true, false, 2 // expect update, no event, final qty = 2
            };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public async Task ReduceQuantityForInventoryItemAsync_TestCases(
            string caseName,
            InventoryItemDto dto,
            InventoryItem existingItem,
            Type expectedException,
            bool shouldUpdate,
            bool shouldRaiseEvent,
            int? expectedFinalQuantity
        )
        {
            // Arrange
            _inventoryRepositoryMock
                .Setup(r => r.GetInventoryItemByItemIdAsync(dto.ItemId))
                .ReturnsAsync(existingItem);

            if (shouldRaiseEvent)
            {
                var mockEvent = new OutOfStockEvent();
                _mapperMock.Setup(m => m.Map<OutOfStockEvent>(It.IsAny<InventoryItem>())).Returns(mockEvent);
            }

            // Act & Assert
            if (expectedException != null)
            {
                await Assert.ThrowsAsync(expectedException, () =>
                    _service.ReduceQuantityForInventoryItemAsync(dto));
            }
            else
            {
                await _service.ReduceQuantityForInventoryItemAsync(dto);

                // Verify final quantity
                Assert.Equal(expectedFinalQuantity, existingItem.Quantity);

                // Verify update
                _inventoryRepositoryMock.Verify(r => r.UpdateInventoryItemAsync(existingItem), Times.Once);

                // Verify event
                if (shouldRaiseEvent)
                {
                    _eventProducerMock.Verify(p => p.PublishOutOfStockEventAsync(It.IsAny<OutOfStockEvent>()), Times.Once);
                }
                else
                {
                    _eventProducerMock.Verify(p => p.PublishOutOfStockEventAsync(It.IsAny<OutOfStockEvent>()), Times.Never);
                }
            }
        }
    }
}
