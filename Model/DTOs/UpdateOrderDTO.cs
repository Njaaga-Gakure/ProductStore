namespace ProductStore.Model.DTOs
{
    public class UpdateOrderDTO
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
    }
}
