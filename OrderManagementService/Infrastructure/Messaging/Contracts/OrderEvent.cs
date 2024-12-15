namespace OrderManagementService.Infrastructure.Messaging.Contracts
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public string BookISBN { get; set; }
    }
}
