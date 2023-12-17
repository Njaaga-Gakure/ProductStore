using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Model;
using ProductStore.Model.DTOs;
using ProductStore.Service.IService;

namespace ProductStore.Service
{
    public class ProductService : IProduct
    {
        private readonly ProductStoreContext _context;

        public ProductService(ProductStoreContext context)
        {
            _context = context;
        }
        public async Task<string> CreateProduct(Product product)
        {
           await _context.Products.AddAsync(product);
           await _context.SaveChangesAsync();
           return "Product Created Successfully :)";
        }


        public async Task<List<Product>> GetAllProducts(int pageNumber)
        {
            var pageSize = 5;
            var skip = (pageNumber - 1) * pageSize;
            var products = await _context.Products
                                .OrderBy(product => product.Name)
                                .Skip(skip)
                                .Take(pageSize)
                                .ToListAsync();
            return products;    
        }

        public async Task<Product> GetProductById(Guid productId)
        {
           var product = await _context.Products
                         .Where(product => product.Id == productId)
                         .FirstOrDefaultAsync();
            return product;
        }

        public async Task<bool> UpdateProduct(Guid productId, AddProductDTO updateProduct)
        {

            var product = await GetProductById(productId);
            if (product != null)
            { 
                product.Name = updateProduct.Name;
                product.Description = updateProduct.Description;
                product.Price = updateProduct.Price;    
                await _context.SaveChangesAsync();  
                return true;    
            }
            return false;

        }
        public async Task<bool> DeleteProduct(Guid productId)
        {
            var product = await GetProductById(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
