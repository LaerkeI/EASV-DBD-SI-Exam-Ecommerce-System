namespace InventoryManagementService.Infrastructure.Messaging.Events
{
    public class CreatedOrderEvent
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderLineEvent> OrderLines { get; set; }
    }
}