using Microsoft.EntityFrameworkCore;
using ProductStore.Model;

namespace ProductStore.Data
{
    public class ProductStoreContext: DbContext
    {
        public ProductStoreContext(DbContextOptions<ProductStoreContext> options): base(options) { }
        public DbSet<User> Users { get; set; }  
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }    
    }
}
