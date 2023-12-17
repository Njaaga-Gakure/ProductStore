using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Model
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }    

        public User User { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public DateTime UpdatedAt { get; set;} = DateTime.Now;  

    }
}
