using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Application.ServicesImplementations;
using BP.Ecommerce.Application.ServicesInterfaces;
using BP.Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IEcommerceService service;

        public OrderController(IEcommerceService service)
        {
            this.service = service;
        }

        [HttpPost("AddProduct")]
        public async Task<OrderProductDto> OrderAddProductAsync(CreateOrderProductDto createOrderProductDto)
        {
            return await service.OrderAddProductAsync(createOrderProductDto);
        }

        [HttpPut("{orderId}/UpdateProduct")]
        public async Task<OrderProductDto> OrderUpdateProductAsync(Guid orderId, UpdateOrderProductDto updateOrderProductDto)
        {
            return await service.OrderUpdateProductAsync(orderId, updateOrderProductDto);
        }

        [HttpDelete("{orderId}/RemoveProduct/{productId}")]
        public async Task<bool> OrderRemoveProductAsync(Guid orderId, Guid productId)
        {
            return await service.OrderRemoveProductAsync(orderId, productId);
        }

        [HttpGet("Show/{orderId}")]
        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            return await service.GetOrderByIdAsync(orderId);
        }
        

        [HttpPost("Pay/{orderId}")]
        public async Task<OrderDto> OrderPayAsync(Guid orderId)
        {
            return await service.OrderPayAsync(orderId);
        }

        [HttpDelete("Cancel/{orderId}")]
        public async Task<bool> OrderCancelAsync(Guid orderId)
        {
            return await service.OrderCancelAsync(orderId);
        }
    }
}
