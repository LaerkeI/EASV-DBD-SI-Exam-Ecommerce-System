namespace OrderManagementService.Domain.Entities
{
    public class OrderLine
    {
        public string ItemId { get; set; }  //Reference to InventoryItem in "InventoryManagementService". Could be ISBN, SKU or Barcode. 
        public int Quantity { get; set; }

        // Foreign Key to Order
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        // Composite key configured in OnModelCreating
    }
}
