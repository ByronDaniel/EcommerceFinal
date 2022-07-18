using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface ICartService
    {
        public Task<OrderProductDto> AddProductAsync(CreateOrderProductDto createOrderProduct);
        public Task<OrderProductDto> UpdateProductAsync(Guid orderId, UpdateOrderProductDto orderProduct);
        public Task<bool> RemoveProductAsync(Guid orderId, Guid productId);
        public Task<OrderDto> GetByIdAsync(Guid orderId);
        public Task<OrderDto> PayAsync(Guid orderId);
        public Task<OrderDto> CancelAsync(Guid orderId);
    }
}
