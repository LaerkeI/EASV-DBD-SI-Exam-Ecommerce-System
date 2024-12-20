namespace InventoryManagementService.Infrastructure.Messaging.Events
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderLineEvent> OrderLines { get; set; }
    }
}