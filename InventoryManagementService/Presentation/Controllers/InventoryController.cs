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

        // GET: api/Inventory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItemDto>> GetInventoryItemById(string id)
        {
            var inventoryItem = await _inventoryService.GetInventoryItemByIdAsync(id);
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
            return CreatedAtAction(nameof(GetInventoryItemById), new { id = createdInventoryItem.Id }, createdInventoryItem);
        }

        // PUT: api/Inventory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(string id, [FromBody] InventoryItemDto inventoryItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != inventoryItemDto.Id)
                return BadRequest("InventoryItem ID mismatch.");

            await _inventoryService.UpdateInventoryItemAsync(inventoryItemDto);
            return NoContent();
        }

        // DELETE: api/Inventory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(string id)
        {
            await _inventoryService.DeleteInventoryItemAsync(id);
            return NoContent();
        }
    }
}
