namespace OrderManagementService.Infrastructure.Messaging.Events
{
    public class OrderLineEvent
    {
        public string ItemId { get; set; } // Contains the same value as ItemId in InventoryManagementService and CatalogManagementService. Could be ISBN, SKU or barcode.
        public int Quantity { get; set; }
    }
}
