namespace OrderManagementService.Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderLineDto> OrderLines { get; set; }
    }
}
