using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Domain.Entities;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IOrderService
    {
        public Task<OrderNewDto> CreateOrderAsync();
        public Task<OrderProductDto> AddProductAsync(Guid orderId, AddProductDto addProductDto);
        public Task<OrderProductDto> UpdateProductAsync(Guid orderId, UpdateOrderProductDto orderProduct);
        public Task<bool> RemoveProductAsync(Guid orderId, Guid productId);
        public Task<OrderDto> GetByIdAsync(Guid orderId);
        public Task<OrderDto> PayAsync(Guid orderId);
        public Task<OrderDto> CancelAsync(Guid orderId);
    }
}
