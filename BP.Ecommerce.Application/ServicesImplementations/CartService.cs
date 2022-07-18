using AutoMapper;
using BP.Ecommerce.Application.DTOs;
using BP.Ecommerce.Application.ServicesInterfaces;
using BP.Ecommerce.Domain.Entities;
using BP.Ecommerce.Domain.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesImplementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository repository;
        private readonly IMapper mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<OrderProductDto> AddProductAsync(CreateOrderProductDto createOrderProduct)
        {
            OrderProduct orderProduct = mapper.Map<OrderProduct>(createOrderProduct);
            OrderProductDto orderProductDto = mapper.Map<OrderProductDto>(await repository.AddProductAsync(orderProduct));
            return orderProductDto;
        }

        public async Task<OrderProductDto> UpdateProductAsync(Guid orderId, UpdateOrderProductDto orderProductDto)
        {
            OrderProduct orderProduct = mapper.Map<OrderProduct>(orderProductDto);
            OrderProductDto orderProductResultDto = mapper.Map<OrderProductDto>(await repository.UpdateProductAsync(orderId, orderProduct));
            return orderProductResultDto;
        }
        
        public async Task<bool> RemoveProductAsync(Guid orderId, Guid productId)
        {
            return await repository.RemoveProductAsync(orderId, productId);
        }
     
        public async Task<OrderDto> GetByIdAsync(Guid orderId)
        {
            var query = repository.GetByIdAsync(orderId);
            if (await query.SingleOrDefaultAsync() == null)
                throw new ArgumentException($"No existe la orden con id: {orderId}");

            return await GetResult(query);
        }

        public async Task<OrderDto> PayAsync(Guid orderId)
        {
            IQueryable<Order> query = await repository.PayAsync(orderId);
            OrderDto orderDto = await GetResult(query);
            await repository.UpdateOrderAsync(orderId, orderDto.Subtotal, orderDto.TotalPrice);
            return orderDto;
        }

        public async Task<OrderDto> CancelAsync(Guid orderId)
        {
            IQueryable<Order> query = await repository.CancelAsync(orderId);
            OrderDto orderDto = await GetResult(query);
            return orderDto;
        }

        public async Task<OrderDto> GetResult(IQueryable<Order> query)
        {
            decimal total = 0;
            OrderDto orderDto = await query.Select(o => new OrderDto()
            {
                Id = o.Id,
                DeliveryMethodId = o.DeliveryMethodId,
                DeliveryMethod = o.DeliveryMethod,
                orderProducts = o.orderProducts.Select(op => new OrderProductResultDto() { Product = op.Product.Name, Price = op.Product.Price, ProductQuantity = op.ProductQuantity, Total = op.Total }).ToList(),
                State = o.State,
                Subtotal = o.Subtotal,
                TotalPrice = o.TotalPrice
            }).SingleOrDefaultAsync();
            foreach (var product in orderDto.orderProducts)
            {
                total += product.Total;
            }
            orderDto.Subtotal = total - (total * (decimal)0.12);
            orderDto.Iva = (total * (decimal)0.12);
            orderDto.TotalPrice = total;
            return orderDto;
        }
    }
}
