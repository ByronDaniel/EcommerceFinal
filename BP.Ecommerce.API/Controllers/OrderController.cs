using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Application.ServicesInterfaces;
using BP.Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService service;

        public OrderController(IOrderService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<OrderNewDto> CreateOrderAsync()
        {
            return await service.CreateOrderAsync();
        }

        [HttpPost("{orderId}/Product")]
        public async Task<OrderProductDto> AddProductAsync(Guid orderId, AddProductDto addProductDto)
        {
            return await service.AddProductAsync(orderId, addProductDto);
        }

        [HttpPut("{orderId}/Product")]
        public async Task<OrderProductDto> UpdateProductAsync(Guid orderId, UpdateOrderProductDto orderProduct)
        {
            return await service.UpdateProductAsync(orderId, orderProduct);
        }

        [HttpDelete("{orderId}/Product/{productId}")]
        public async Task<bool> RemoveProductAsync(Guid orderId, Guid productId)
        {
            return await service.RemoveProductAsync(orderId, productId);
        }

        [HttpGet("Show/{orderId}")]
        public async Task<OrderDto> GetByIdAsync(Guid orderId)
        {
            return await service.GetByIdAsync(orderId);
        }

        [HttpPut("Pay/{orderId}")]
        public async Task<OrderDto> PayAsync(Guid orderId)
        {
            return await service.PayAsync(orderId);
        }

        [HttpPut("Cancel/{orderId}")]
        public async Task<OrderDto> CancelAsync(Guid orderId)
        {
            return await service.CancelAsync(orderId);
        }
    }
}
