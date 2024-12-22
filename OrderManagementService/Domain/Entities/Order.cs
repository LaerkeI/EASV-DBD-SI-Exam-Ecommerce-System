namespace OrderManagementService.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        // List of OrderLines representing each item purchased
        public List<OrderLine> OrderLines { get; set; }
    }
}
