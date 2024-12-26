namespace InventoryManagementService.Application.DTOs
{
    public class InventoryItemDto
    {
        public string ItemId { get; set; } //The same value is in OrderManagementService and CatalogManagementService. Could be ISBN, SKU or Barcode.
        public int Quantity { get; set; }
    }
}
