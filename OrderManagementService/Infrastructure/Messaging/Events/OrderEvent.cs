namespace OrderManagementService.Infrastructure.Messaging.Events
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public string BookISBN { get; set; }
    }
}
