namespace InventoryManagementService.Infrastructure.Messaging.Events
{
    public class OrderLineEvent
    {
        public string ItemId { get; set; }  //This could be ISBN/SKU/Barcode or what the company chooses
        public int Quantity { get; set; }
    }
}
