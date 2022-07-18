using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Application.ServicesInterfaces;
using BP.Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BP.Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICartService service;

        public CarController(ICartService service)
        {
            this.service = service;
        }

        [HttpPost("AddProduct")]
        public async Task<OrderProductDto> AddProductAsync(CreateOrderProductDto createOrderProduct)
        {
            return await service.AddProductAsync(createOrderProduct);
        }

        [HttpPut("{orderId}/UpdateProduct")]
        public async Task<OrderProductDto> UpdateProductAsync(Guid orderId, UpdateOrderProductDto orderProduct)
        {
            return await service.UpdateProductAsync(orderId, orderProduct);
        }
        [HttpDelete("{orderId}/RemoverProduct/{productId}")]
        public async Task<bool> RemoveProductAsync(Guid orderId, Guid productId)
        {
            return await service.RemoveProductAsync(orderId, productId);
        }
        [HttpGet("{orderId}")]
        public async Task<OrderDto> GetByIdAsync(Guid orderId)
        {
            return await service.GetByIdAsync(orderId);
        }

        [HttpPut("{orderId}/Pay")]
        public async Task<OrderDto> PayAsync(Guid orderId)
        {
            return await service.PayAsync(orderId);
        }

        [HttpPut("{orderId}/Cancel")]
        public async Task<OrderDto> CancelAsync(Guid orderId)
        {
            return await service.CancelAsync(orderId);
        }
    }
}
