using BP.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Domain.RepositoriesInterfaces
{
    public interface IEcommerceRepository
    {
        /// <summary>
        /// Agrega productos a la orden
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Task<OrderProduct> OrderAddProductAsync(OrderProduct orderProduct);

        /// <summary>
        /// Eliminar producto a la orden
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<bool> OrderRemoveProductAsync(Guid orderId, Guid productId);

        /// <summary>
        /// Actualizar cantidad de productos en la orden
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Task<OrderProduct> OrderUpdateProductAsync(Guid orderId, OrderProduct orderProduct);

        /// <summary>
        /// Crear Orden
        /// </summary>
        /// <returns></returns>
        public Task<Order> CreateOrderAsync();

        /// <summary>
        /// Mostrar Orden
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IQueryable<Order>> GetOrderByIdAsync(Guid id);

        /// <summary>
        /// Pagar Orden
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Order> OrderPayAsync(Guid id);

        /// <summary>
        /// Cancelar Orden y rembolso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> OrderCancelAsync(Guid id);
    }
}
