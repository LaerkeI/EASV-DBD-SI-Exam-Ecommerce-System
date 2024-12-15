using Shared.DTOs;

namespace OrderManagementService.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        // List of OrderLines representing each Ebook purchased
        public List<OrderLine> OrderLines { get; set; }
    }
}
