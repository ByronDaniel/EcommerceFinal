using BP.Ecommerce.Domain.Entities;

namespace BP.Ecommerce.Domain.RepositoriesInterfaces
{
    public interface IOrderRepository
    {
        public Task<Order> CreateOrderAsync();
        public Task<OrderProduct> AddProductAsync(Guid orderId, OrderProduct orderProduct);
        public Task<OrderProduct> UpdateProductAsync(Guid orderId, OrderProduct orderProduct);
        public Task<bool> RemoveProductAsync(Guid orderId, Guid productId);
        public IQueryable<Order> GetOrderByIdAsync(Guid orderId);
        public Task<Order> UpdateOrderAsync(Guid orderId, decimal subtotal, decimal totalPrice);
        public Task<IQueryable<Order>> PayAsync(Guid orderId);
        public Task<IQueryable<Order>> CancelAsync(Guid orderId);
    }
}
