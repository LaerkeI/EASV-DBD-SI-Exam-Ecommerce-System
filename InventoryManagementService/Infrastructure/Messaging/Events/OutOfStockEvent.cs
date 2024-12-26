namespace InventoryManagementService.Infrastructure.Messaging.Events
{
    public class OutOfStockEvent
    {
        public string ItemId { get; set; } // Contains the same value as ItemId in OrderManagementService and CatalogManagementService. Could be ISBN, SKU or barcode.
    }
}
