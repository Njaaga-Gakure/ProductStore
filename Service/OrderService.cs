using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Model;
using ProductStore.Model.DTOs;
using ProductStore.Service.IService;

namespace ProductStore.Service
{
    public class OrderService : IOrder
    {
        private readonly ProductStoreContext _context;


        public OrderService(ProductStoreContext context)
        {
            _context = context; 
        }
        public async Task<string> CreateOrder(Order order)
        {
           await _context.Orders.AddAsync(order);
           await _context.SaveChangesAsync();  
           return "Order Added Successfully :)";
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _context.Orders
                             .Include(order => order.Product)
                             .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
           var existingOrder = await (from order in _context.Orders
                                      where order.Id == orderId
                                      select order)
                                      .Include(order => order.Product)
                                      .FirstOrDefaultAsync();
            return existingOrder; 
        }

        public async Task<bool> UpdateOrder(Guid orderId, UpdateOrderDTO updateOrder)
        {
            var order = await GetOrderById(orderId);
            if (order != null) 
            {
                order.ProductId = updateOrder.ProductId;
                order.UserId = updateOrder.UserId;
                order.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteOrder(Guid orderId)
        {
            var order = await GetOrderById(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
