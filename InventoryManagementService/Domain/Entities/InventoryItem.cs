namespace InventoryManagementService.Domain.Entities
{
    public class InventoryItem
    {
        public string ItemId { get; set; } // Contains the same value as ItemId in OrderManagementService and CatalogManagementService. Could be ISBN, SKU or barcode.
        public int Quantity { get; set; }
    }
}
