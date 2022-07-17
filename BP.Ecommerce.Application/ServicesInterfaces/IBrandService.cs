using BP.Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IBrandService
    {
        public Task<List<BrandDto>> GetAllAsync();
        public Task<BrandDto> GetByIdAsync(Guid id);
        public Task<BrandDto> PostAsync(CreateBrandDto createBrandDto);
        public Task<BrandDto> PutAsync(BrandDto brandDto);
        public Task<bool> DeleteAsync(Guid id);
    }
}
