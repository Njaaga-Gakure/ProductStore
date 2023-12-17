using ProductStore.Model;
using ProductStore.Model.DTOs;

namespace ProductStore.Service.IService
{
    public interface IProduct
    {
        Task<string> CreateProduct(Product product);
        Task<List<Product>> GetAllProducts(int pageNumber);

        Task<Product> GetProductById(Guid productId);

        Task<bool> UpdateProduct(Guid productId, AddProductDTO updateProduct);

        Task<bool> DeleteProduct(Guid productId);
    }
}
