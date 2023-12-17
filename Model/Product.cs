using System.ComponentModel.DataAnnotations;

namespace ProductStore.Model
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Price { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();    
    }
}
