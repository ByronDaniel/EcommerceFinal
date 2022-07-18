using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IEcommerceService
    {
        public Task<OrderProductDto> OrderAddProductAsync(CreateOrderProductDto createOrderProductDto);
        public Task<bool> OrderRemoveProductAsync(Guid orderId, Guid productId);
        public Task<OrderProductDto> OrderUpdateProductAsync(Guid orderId, UpdateOrderProductDto updateOrderProductDto);
        public Task<OrderDto> GetOrderByIdAsync(Guid id);
        public Task<OrderDto> OrderPayAsync(Guid id);
        public Task<bool> OrderCancelAsync(Guid id);

    }
}
