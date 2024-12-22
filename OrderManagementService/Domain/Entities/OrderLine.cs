namespace OrderManagementService.Domain.Entities
{
    public class OrderLine
    {
        public string ItemId { get; set; }  //Reference to Item in "CatalogManagementService". Could be ISBN/SKU/Barcode. 
        public int Quantity { get; set; }

        // Foreign Key to Order
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        // Composite key configured in OnModelCreating
    }
}
