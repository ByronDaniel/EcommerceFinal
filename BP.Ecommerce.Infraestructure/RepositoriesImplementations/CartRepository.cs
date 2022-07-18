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
    public class CartRepository : ICartRepository
    {
        private readonly EcommerceDbContext context;

        public CartRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public async Task<OrderProduct> AddProductAsync(OrderProduct orderProduct)
        {
            Product product = await ValidateQuantityStock(orderProduct);
            //si orderProduct.OrderId es vacio crear orden, si no es vacio y la orden existe agrega el producto a la orden
            Order order = new Order();
            OrderProduct orderProductExist = new OrderProduct();
            if (orderProduct.OrderId == Guid.Empty)
            {
                order = await CreateOrderAsync();
                orderProduct.OrderId = order.Id;
                orderProduct.Total = product.Price * orderProduct.ProductQuantity;
                //Agregamos orderProduct y guardar cambios
                await context.OrderProducts.AddAsync(orderProduct);
                await context.SaveChangesAsync();
            }
            else
            {
                Order orderFind = await FindOrderAsync(orderProduct);
                orderProductExist = await context.OrderProducts.Where(o => o.OrderId == orderFind.Id && o.ProductId == product.Id).SingleOrDefaultAsync();
                //Modificamos la cantidad si existe la orden con producto
                if (orderProductExist != null)
                {
                    orderProductExist.ProductQuantity += orderProduct.ProductQuantity;
                    orderProductExist.Total += product.Price * orderProduct.ProductQuantity;
                    context.Entry(orderProductExist).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                else
                {
                    orderProduct.Total = product.Price * orderProduct.ProductQuantity;
                    await context.OrderProducts.AddAsync(orderProduct);
                    await context.SaveChangesAsync();
                }
            }
            //Quitar cantidad seleccionada de productos al stock del producto y actualizar producto
            product.Stock -= orderProduct.ProductQuantity;
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            //Si no existe orden retorna la orden creada, sino retorna orden existente
            if (order.Id != Guid.Empty || orderProductExist == null)
            {
                return orderProduct;
            }
            return orderProductExist;
        }

        public async Task<OrderProduct> UpdateProductAsync(Guid orderId, OrderProduct orderProduct)
        {
            OrderProduct orderProductFind = await context.OrderProducts.Where(o => o.ProductId == orderProduct.ProductId && o.OrderId == orderId).SingleOrDefaultAsync();
            Product product = await ValidateQuantityStock(orderProduct, 'U');
            //Si voy a aumentar cantidad de producto, restar stock del producto,
            //Sino si voy a reducir la cantidad del producto, aumentar stock del producto
            var stockRestado = (orderProduct.ProductQuantity - orderProductFind.ProductQuantity);
            var stockAumentado = (orderProductFind.ProductQuantity - orderProduct.ProductQuantity);
            if (orderProductFind.ProductQuantity < orderProduct.ProductQuantity)
            {
                if(product.Stock - stockRestado < 0)
                    throw new ArgumentException($"No hay suficiente stock del producto, cantidad disponible: {product.Stock}, cantidad seleccionada: {orderProduct.ProductQuantity}");
                
                product.Stock -= stockRestado;
            }
            else
            {
                if (orderProductFind.ProductQuantity < orderProduct.ProductQuantity)
                    throw new ArgumentException($"No se permite devolver mas productos de los adquiridos, Productos adquiridos {orderProductFind.ProductQuantity}, productos devueltos {orderProduct.ProductQuantity}");

                product.Stock += stockAumentado;
            }
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            
            orderProductFind.ProductQuantity = orderProduct.ProductQuantity;
            orderProductFind.Total = product.Price * orderProductFind.ProductQuantity;
            
            context.Entry(orderProductFind).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return orderProductFind;
        }
        
        public async Task<bool> RemoveProductAsync(Guid orderId, Guid productId)
        {
            //devolver stock del producto 
            OrderProduct orderProduct = await context.OrderProducts.Where(o=> o.OrderId == orderId && o.ProductId == productId).SingleOrDefaultAsync();
            if (orderProduct == null)
                throw new ArgumentException($"No existe la orden {orderId} con el producto {productId}");
            context.OrderProducts.Remove(orderProduct);
            await context.SaveChangesAsync();

            var quantity = orderProduct.ProductQuantity;
            Product product = await  context.Products.FindAsync(productId);
            if (product == null)
                throw new ArgumentException($"No existe el producto {productId}");
            product.Stock += quantity;
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Order> GetByIdAsync(Guid orderId)
        {
            return context.Order.Where(o=> o.Id == orderId).AsQueryable();
        }
        
        public async Task<IQueryable<Order>> PayAsync(Guid orderId)
        {
            IQueryable<Order> query = context.Order.Where(o => o.Id == orderId).AsQueryable();
            Order order = await query.SingleOrDefaultAsync();
            if (order == null)
                throw new ArgumentException($"No existe la orden con id: {orderId}");

            order.State = Status.Pagado.ToString();
            order.DateModification = DateTime.Now;
            context.Entry(order).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return query;
        }
        
        public async Task<IQueryable<Order>> CancelAsync(Guid orderId)
        {
            var query = context.Order.Where(o=> o.Id == orderId).AsQueryable();
            Order order = await query.SingleOrDefaultAsync();
            if (order == null)
                throw new ArgumentException($"No existe la orden con id: {orderId}");
            if (order.State == Status.Pagado.ToString() || order.State == Status.Pendiente.ToString())
            {
                List<OrderProduct> orderProducts = await context.OrderProducts.Where(o => o.OrderId == orderId).ToListAsync();
                Product productSelected = new Product();
                foreach (var orderProduct in orderProducts) {
                    productSelected = await context.Products.Where(o=> o.Id == orderProduct.ProductId).SingleAsync();
                    productSelected.Stock += orderProduct.ProductQuantity;
                    context.Entry(productSelected).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ArgumentException($"No se puede cancelar o rembolsar la orden porque el estado actual es: {order.State}");
            }
            if (order.State == Status.Pendiente.ToString())
            {
                order.State = Status.Cancelado.ToString();
            }
            else if (order.State == Status.Pagado.ToString())
            {
                order.State = Status.Rembolsado.ToString();
            }
            context.Entry(order).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return query;
        }

        public async Task<Product> ValidateQuantityStock(OrderProduct orderProduct, char operation = 'C')
        {
            //Validar que la cantidad sea mayor a 0
            if (orderProduct.ProductQuantity <= 0)
                throw new ArgumentException("La cantidad seleccionada debe ser mayor a 0");

            //Consultar y validar si existe el producto seleccionado
            Product product = await context.Products.FindAsync(orderProduct.ProductId);
            if (product == null)
                throw new ArgumentException($"No existe el producto con Id: {orderProduct.ProductId}");
            if (operation != 'U')
            {
                //Validar que la cantidad seleccionada sea menor al stock del producto, y que el stock es 0
                if (product.Stock < orderProduct.ProductQuantity || product.Stock == 0)
                    throw new ArgumentException($"No suficiente stock del producto, cantidad disponible: {product.Stock}, cantidad seleccionada: {orderProduct.ProductQuantity}");
            }

            return product;
        }

        public async Task<Order> CreateOrderAsync()
        {
            //Creando la nueva orden
            Order order = new Order()
            {
                State = Status.Pendiente.ToString(),
                Subtotal = 0,
                TotalPrice = 0
            };
            await context.Order.AddAsync(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> FindOrderAsync(OrderProduct orderProduct)
        {
            Order orderFind = await context.Order.FindAsync(orderProduct.OrderId);
            if (orderFind == null)
                throw new ArgumentException($"No existe la orden con id: {orderProduct.OrderId}");
            return orderFind;
        }

        public async Task<Order> UpdateOrderAsync(Guid orderId ,decimal subtotal, decimal totalPrice)
        {
            Order order = await context.Order.FindAsync(orderId);
            if (order == null)
                throw new ArgumentException($"No existe la orden con id: {orderId}");

            order.Subtotal = subtotal;
            order.TotalPrice = totalPrice;
            context.Entry(order).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return order;
        }
    }
}
