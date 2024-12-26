using OrderManagementService.Domain.Entities;

namespace OrderManagementService.Infrastructure.Messaging.Events
{
    public class CreatedOrderEvent
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // List of OrderLines representing each Ebook purchased
        public List<OrderLineEvent> OrderLines { get; set; }
    }
}
