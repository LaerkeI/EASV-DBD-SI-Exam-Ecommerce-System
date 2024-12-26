using InventoryManagementService.Application.DTOs;
using InventoryManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems()
        {
            var inventoryItems = await _inventoryService.GetInventoryItemsAsync();
            return Ok(inventoryItems);
        }

        // GET: api/Inventory/{itemId}
        [HttpGet("{itemId}")]
        public async Task<ActionResult<InventoryItemDto>> GetInventoryItemByItemId(string itemId)
        {
            var inventoryItem = await _inventoryService.GetInventoryItemByItemIdAsync(itemId);
            if (inventoryItem == null)
                return NotFound();

            return Ok(inventoryItem);
        }

        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> CreateInventoryItem([FromBody] InventoryItemDto inventoryItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdInventoryItem = await _inventoryService.CreateInventoryItemAsync(inventoryItemDto);

            // Return CreatedAt route for RESTful conventions
            return CreatedAtAction(nameof(GetInventoryItemByItemId), new { itemId = createdInventoryItem.ItemId }, createdInventoryItem);
        }

        // PUT: api/Inventory/{itemId}
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateInventoryItem(string itemId, [FromBody] InventoryItemDto inventoryItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (itemId != inventoryItemDto.ItemId)
                return BadRequest("InventoryItem ItemId mismatch.");

            await _inventoryService.UpdateInventoryItemAsync(inventoryItemDto);
            return NoContent();
        }

        // DELETE: api/Inventory/{itemId}
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteInventoryItem(string itemId)
        {
            await _inventoryService.DeleteInventoryItemAsync(itemId);
            return NoContent();
        }
    }
}
