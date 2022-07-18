using BP.Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IProductTypeService
    {
        public Task<List<ProductTypeDto>> GetAllAsync(string? search, string? sort, string? order, int? limit, int? offset);
        public Task<ProductTypeDto> GetByIdAsync(Guid id);
        public Task<ProductTypeDto> PostAsync(CreateProductTypeDto createProductTypeDto);
        public Task<ProductTypeDto> PutAsync(ProductTypeDto productTypeDto);
        public Task<bool> DeleteAsync(Guid id);
    }
}
