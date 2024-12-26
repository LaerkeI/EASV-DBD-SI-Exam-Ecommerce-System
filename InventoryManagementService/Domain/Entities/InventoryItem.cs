namespace InventoryManagementService.Domain.Entities
{
    public class InventoryItem
    {
        public string ItemId { get; set; } //Same property as in OrderManagementService and CatalogManagementService. Could be ISBN, SKU or Barcode.
        public int Quantity { get; set; }
    }
}
