namespace OrderManagementService.Infrastructure.Messaging.Events
{
    public class OrderLineEvent
    {
        public string ISBN { get; set; }
        public int Quantity { get; set; }
    }
}
