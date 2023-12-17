namespace ProductStore.Model.DTOs
{
    public class OrderDTO
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderCreatedAt { get; set; }
        public DateTime OrderUpdatedAt { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
        public string ProductDescription { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
    }
}
