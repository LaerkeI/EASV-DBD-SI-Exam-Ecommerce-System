namespace Shared.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BookISBN { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
