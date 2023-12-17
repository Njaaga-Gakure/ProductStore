using ProductStore.Model.DTOs;
using ProductStore.Model;

namespace ProductStore.Service.IService
{
    public interface IOrder
    {
        Task<string> CreateOrder(Order order);
        Task<List<Order>> GetAllOrders();

        Task<Order> GetOrderById(Guid orderId);

        Task<bool> UpdateOrder(Guid orderId, UpdateOrderDTO updateOrder);

        Task<bool> DeleteOrder(Guid orderId);
    }
}
