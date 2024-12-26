namespace OrderManagementService.Domain.Entities
{
    public class OrderLine
    {
        public string ItemId { get; set; }  // Part of composite key. A logical reference to a CatalogItem even though there is no FK relation as with Order.
        public int Quantity { get; set; }
        public int OrderId { get; set; } // Part of composite key. A reference to an Order. 
        public Order Order { get; set; }
        
    }
}
