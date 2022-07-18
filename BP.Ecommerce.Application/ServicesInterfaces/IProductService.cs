using BP.Ecommerce.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Obtiene todas las tipo de productos
        /// </summary>
        /// <returns></returns>
        Task<List<ProductDto>> GetAllAsync(string search, int limit, int offset, string sort, string order);

        /// <summary>
        /// Obtiene tipo de producto por Id
        /// </summary>
        /// <returns></returns>
        Task<ProductDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Agrega tipo de producto
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        Task<ProductDto> PostAsync(CreateProductDto createProductDto);

        /// <summary>
        /// Edita tipo de producto
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        Task<ProductDto> PutAsync(ProductDto productDto);

        /// <summary>
        /// Elimina tipo de producto por Id
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(Guid id);

    }
}
