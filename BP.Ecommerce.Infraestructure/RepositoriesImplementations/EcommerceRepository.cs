using BP.Ecommerce.Domain.Entities;
using BP.Ecommerce.Domain.RepositoriesInterfaces;
using BP.Ecommerce.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Infraestructure.RepositoriesImplementations
{
    public class EcommerceRepository : IEcommerceRepository
    {
        private readonly EcommerceDbContext context;

        public EcommerceRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public async Task<IQueryable<Order>> GetOrderByIdAsync(Guid id)
        {
            var order = context.Order.Where(o=> o.Id == id).AsQueryable();
            if (await order.SingleOrDefaultAsync() == null)
                throw new ArgumentException($"No existe la orden");

            return order;
        }
        
        public async Task<OrderProduct> OrderAddProductAsync(OrderProduct orderProduct)
        {
            if (orderProduct.ProductQuantity <= 0)
                throw new ArgumentException($"La cantidad debe ser mayor a 0");

            Product product = await context.Products.FindAsync(orderProduct.ProductId);

            if (product == null)
                throw new ArgumentException($"No existe producto con id {orderProduct.ProductId}");

            if (product.Stock == 0)
                throw new ArgumentException($"No hay stock del producto {product.Name}");

            if (product.Stock < orderProduct.ProductQuantity)
                throw new ArgumentException($"La cantidad solicitada sobrepasa el stock del producto {product.Name}, cantidad disponible: {product.Stock}");

            Order orderGenerated = new Order();
            if (orderProduct.OrderId == Guid.Empty)
            {
                orderGenerated = await CreateOrderAsync();
            }
            else
            {
                Order order = await context.Order.FindAsync(orderProduct.OrderId);
                if (order == null)
                    throw new ArgumentException($"No existe la orden con id {orderProduct.OrderId}");

                orderGenerated.Id = (Guid)orderProduct.OrderId;
            }

            OrderProduct orderProductFind = await context.OrderProducts.Where(o => o.ProductId == orderProduct.ProductId && o.OrderId == orderProduct.OrderId).SingleOrDefaultAsync();
            if (orderProductFind == null)
            {
                if (product.Stock < orderProduct.ProductQuantity)
                    throw new ArgumentException($"La cantidad seleccionada del producto {orderProduct.Product.Name} sobrepasa al stock del producto, disponibles: {product.Stock}");

                OrderProduct orderProductNew = new OrderProduct()
                {
                    ProductQuantity = orderProduct.ProductQuantity,
                    ProductId = orderProduct.ProductId,
                    OrderId = orderGenerated.Id
                };
                await context.OrderProducts.AddAsync(orderProductNew);
                await context.SaveChangesAsync();
                return orderProductNew;
            }
            else
            {
                orderProductFind.ProductQuantity += orderProduct.ProductQuantity;

                if (product.Stock < orderProductFind.ProductQuantity)
                    throw new ArgumentException($"La cantidad seleccionada del producto {product.Name} sobrepasa al stock del producto, disponibles: {product.Stock}, seleccionados: {orderProductFind.ProductQuantity}");


                context.Entry(orderProductFind).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return orderProductFind;
            }
        }

        public async Task<OrderProduct> OrderUpdateProductAsync(Guid orderId, OrderProduct updateOrderProductDto)
        {
            OrderProduct orderProduct = await context.OrderProducts.Where(o => o.ProductId == updateOrderProductDto.ProductId && o.OrderId == orderId).SingleOrDefaultAsync();
            Product product = await context.Products.Where(p => p.Id == updateOrderProductDto.ProductId).SingleOrDefaultAsync();
            if (orderProduct == null)
                throw new ArgumentException($"No existe el producto en la orden");

            if (updateOrderProductDto.ProductQuantity <= 0)
                throw new ArgumentException($"La cantidad debe ser mayor a 0");

            if (product.Stock < updateOrderProductDto.ProductQuantity)
                throw new ArgumentException($"La cantidad seleccionada del producto {orderProduct.Product.Name} sobrepasa al stock del producto, disponibles: {product.Stock}");
           
            orderProduct.ProductQuantity = updateOrderProductDto.ProductQuantity;
            context.Entry(orderProduct).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return orderProduct;
        }

        public async Task<bool> OrderRemoveProductAsync(Guid orderId, Guid productId)
        {
            OrderProduct orderProduct = await context.OrderProducts.Where(o => o.ProductId == productId && o.OrderId == orderId).SingleOrDefaultAsync();
            if (orderProduct == null)
                throw new ArgumentException($"No existe el producto en la orden");

            context.OrderProducts.Remove(orderProduct);
            return true;
        }

        public async Task<Order> CreateOrderAsync()
        {
            Order order = new Order();
            order.State = Status.Pendiente.ToString();
            await context.Order.AddAsync(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> OrderPayAsync(Guid id)
        {
            Order order = await context.Order.AsQueryable().Where(o => o.Id == id).Select(p => new Order()
            {
                Id = p.Id,
                DeliveryMethodId = p.DeliveryMethodId,
                State = p.State,
                orderProducts = p.orderProducts.Select(o=> new OrderProduct() { Id = o.Id, Product = o.Product, ProductQuantity = o.ProductQuantity}).ToList(),
                Subtotal = p.Subtotal,
                TotalPrice = p.TotalPrice
            }).SingleOrDefaultAsync();
            if (order == null)
                throw new ArgumentException($"No existe la orden con id {id}");
            if (order.State == Status.Pendiente.ToString())
            {
                List<OrderProduct> orderProducts = order.orderProducts ;
                var productQuantity = 0;
                decimal price = 0;
                decimal totalPay = 0;
                var productSelected = new Product();
                foreach (var orderProduct in orderProducts)
                {
                    if (orderProduct.Product.Stock < orderProduct.ProductQuantity)
                    {
                        throw new ArgumentException($"La cantidad seleccionada del producto {orderProduct.Product.Name} sobrepasa al stock del producto, disponibles: {orderProduct.Product.Stock}");
                    }
                    productQuantity = orderProduct.ProductQuantity;
                    price = orderProduct.Product.Price;

                    totalPay += (productQuantity * price);
                    productSelected = orderProduct.Product;

                    productSelected.Stock -= productQuantity;
                    context.Entry(productSelected).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                decimal porcentajeIva = (decimal)1.12;
                order.Subtotal = totalPay / porcentajeIva;
                order.TotalPrice = totalPay;
                order.State= Status.Pagado.ToString();
                context.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return order;
            }
            else
            {
                throw new ArgumentException($"El estado de la orden es: {order.State} no se pude pagar");
            }
        }

        public async Task<bool> OrderCancelAsync(Guid id)
        {
            Order order = await context.Order.AsQueryable().Where(o => o.Id == id).Select(p => new Order()
            {
                Id = p.Id,
                DeliveryMethodId = p.DeliveryMethodId,
                State = p.State,
                orderProducts = p.orderProducts.Select(o => new OrderProduct() { Id = o.Id, Product = o.Product, ProductQuantity = o.ProductQuantity }).ToList(),
                Subtotal = p.Subtotal,
                TotalPrice = p.TotalPrice
            }).SingleOrDefaultAsync();

            if (order == null)
                throw new ArgumentException($"No existe la orden con id {id}");

            if (order.State== Status.Pagado.ToString())
            {
                List<OrderProduct> orderProducts = order.orderProducts;

                var productSelected = new Product();
                foreach (var orderProduct in orderProducts)
                {
                    productSelected = orderProduct.Product;

                    productSelected.Stock += orderProduct.ProductQuantity;
                    context.Entry(productSelected).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                order.State= Status.Rembolsado.ToString();
            }
            else
            {
                order.State= Status.Cancelado.ToString();
            }
            context.Entry(order).State = EntityState.Modified;
            context.SaveChangesAsync();
            return true;
        }
    }
}
