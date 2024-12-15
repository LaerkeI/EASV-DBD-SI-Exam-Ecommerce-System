namespace OrderManagementService.Domain.Entities
{
    public class OrderLine
    {
        public int Id { get; set; }  // Primary Key for OrderLine
        public int OrderId { get; set; }  // Foreign Key to Order
        public Order Order { get; set; }  // Navigation Property to Order
        public string ISBN { get; set; }  // ISBN of the Ebook
        public int Quantity { get; set; }  // Quantity of the Ebook purchased
    }
}
