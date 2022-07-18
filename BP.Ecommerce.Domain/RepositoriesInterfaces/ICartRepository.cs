using BP.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.RepositoriesInterfaces
{
    public interface ICartRepository
    {
        public Task<OrderProduct> AddProductAsync(OrderProduct orderProduct);
        public Task<OrderProduct> UpdateProductAsync(Guid orderId, OrderProduct orderProduct);
        public Task<bool> RemoveProductAsync(Guid orderId, Guid productId);
        public IQueryable<Order> GetByIdAsync(Guid orderId);
        public Task<IQueryable<Order>> PayAsync(Guid orderId);
        public Task<IQueryable<Order>> CancelAsync(Guid orderId);
        public Task<Order> UpdateOrderAsync(Guid orderId, decimal subtotal, decimal totalPrice);
    }
}
