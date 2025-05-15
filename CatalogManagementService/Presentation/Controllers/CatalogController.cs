using Microsoft.AspNetCore.Mvc;
using CatalogManagementService.Application.Services;
using CatalogManagementService.Application.DTOs;

namespace CatalogManagementService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // GET: api/Catalog/catalog
        [HttpGet("catalog")]
        public async Task<ActionResult<List<CatalogItemDto>>> GetCatalog()
        {
            var catalogItems = await _catalogService.GetCatalogAsync();
            return Ok(catalogItems);
        }

        // GET: api/Catalog/
        [HttpGet]
        public async Task<ActionResult<List<CatalogItemDto>>> GetCatalogItemsAsync()
        {
            var catalogItems = await _catalogService.GetCatalogItemsAsync();
            return Ok(catalogItems);
        }

        // GET: api/Catalog/{itemId}
        [HttpGet("{itemId}")]
        public async Task<ActionResult<CatalogItemDto>> GetCatalogItemByItemIdAsync(string itemId)
        {
            var catalogItemDto = await _catalogService.GetCatalogItemByItemIdAsync(itemId);
            if (catalogItemDto == null)
            {
                return NotFound($"Catalog item with ItemId {itemId} not found.");
            }
            return Ok(catalogItemDto);
        }

        // POST: api/Catalog
        [HttpPost]
        public async Task<IActionResult> CreateCatalogItemAsync([FromBody] CatalogItemDto catalogItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _catalogService.CreateCatalogItemAsync(catalogItemDto);
            return Ok();
        }

        // PUT: api/Catalog/{itemId}
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateCatalogItemAsync(string itemId, [FromBody] CatalogItemDto updatedCatalogItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _catalogService.UpdateCatalogItemAsync(itemId, updatedCatalogItemDto);
            if (!success)
            {
                return NotFound($"Catalog item with ItemId {itemId} not found.");
            }

            return NoContent();
        }

        // PATCH: api/Catalog/{itemId}/availability
        [HttpPatch("{itemId}/availability")]
        public async Task<IActionResult> UpdateAvailabilityOfCatalogItemAsync(string itemId)
        {
            var success = await _catalogService.UpdateAvailabilityOfCatalogItemAsync(itemId);
            if (!success)
            {
                return NotFound($"Catalog item with ItemId {itemId} not found.");
            }

            return NoContent();
        }

        // DELETE: api/Catalog/{itemId}
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteCatalogItemAsync(string itemId)
        {
            var success = await _catalogService.DeleteCatalogItemAsync(itemId);
            if (!success)
            {
                return NotFound($"Catalog item with ItemId {itemId} not found.");
            }

            return NoContent();
        }
    }
}