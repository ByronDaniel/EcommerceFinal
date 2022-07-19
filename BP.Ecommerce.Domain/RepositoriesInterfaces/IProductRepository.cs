using BP.Ecommerce.Domain.Entities;

namespace BP.Ecommerce.Domain.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<IQueryable<Product>> GetQueryable(string search, int limit, int offset, string sort, string order);
        Task<IQueryable<Product>> GetQueryableByIdAsync(Guid id);
        Task<IQueryable<Product>> PostAsync(Product product);
        Task<IQueryable<Product>> PutAsync(Product product);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
