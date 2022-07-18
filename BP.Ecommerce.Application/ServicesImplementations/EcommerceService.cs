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
    public class EcommerceService : IEcommerceService
    {
        private readonly IEcommerceRepository repository;
        private readonly IMapper mapper;

        public EcommerceService(IEcommerceRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<OrderProductDto> OrderAddProductAsync(CreateOrderProductDto createOrderProductDto)
        {
            OrderProduct orderProduct = mapper.Map<OrderProduct>(createOrderProductDto);
            return mapper.Map<OrderProductDto>(await repository.OrderAddProductAsync(orderProduct));
        }

        public async Task<bool> OrderCancelAsync(Guid id)
        {
            return await repository.OrderCancelAsync(id);
        }


        public async Task<bool> OrderRemoveProductAsync(Guid orderId, Guid productId)
        {
            return await repository.OrderRemoveProductAsync(orderId, productId);
        }

        public async Task<OrderDto> OrderPayAsync(Guid id)
        {
            return mapper.Map<OrderDto>(await repository.OrderPayAsync(id));
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var query= await repository.GetOrderByIdAsync(id);
            OrderDto order = await query.Select(o => new OrderDto()
            {
                DeliveryMethodId = o.DeliveryMethodId,
                DeliveryMethod = o.DeliveryMethod,
                Id = o.Id,
                State = o.State,
                Subtotal = o.Subtotal,
                TotalPrice = o.TotalPrice,
                orderProducts = o.orderProducts.Select(p => new OrderProductDto()
                {
                    ProductId = p.ProductId,
                    OrderId = p.OrderId,
                    ProductQuantity = p.ProductQuantity
                }).ToList()
            }).SingleOrDefaultAsync();
            return order;
        }

        public async Task<OrderProductDto> OrderUpdateProductAsync(Guid orderId, UpdateOrderProductDto updateOrderProductDto)
        {
            OrderProduct orderProduct = mapper.Map<OrderProduct>(updateOrderProductDto);
            return mapper.Map<OrderProductDto>(await repository.OrderUpdateProductAsync(orderId, orderProduct));
        }
    }
}
