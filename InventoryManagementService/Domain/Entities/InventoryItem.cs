namespace InventoryManagementService.Domain.Entities
{
    public class InventoryItem
    {
        public string Id { get; set; } //ISBN/SKU/Barcode
        public int Quantity { get; set; }
    }
}
