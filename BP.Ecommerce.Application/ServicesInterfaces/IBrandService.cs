using BP.Ecommerce.Application.DTOs;

namespace BP.Ecommerce.Application.ServicesInterfaces
{
    public interface IBrandService
    {
        public Task<List<BrandDto>> GetAllAsync(string? search, string? sort, string? order, int? limit, int? offset);
        public Task<BrandDto> GetByIdAsync(Guid id);
        public Task<BrandDto> PostAsync(CreateBrandDto createBrandDto);
        public Task<BrandDto> PutAsync(BrandDto brandDto);
        public Task<bool> DeleteAsync(Guid id);
    }
}
