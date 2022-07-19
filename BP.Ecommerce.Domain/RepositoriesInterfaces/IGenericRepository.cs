namespace BP.Ecommerce.Domain.RepositoriesInterfaces
{
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns>List of objects</returns>
        public Task<List<T>> GetAllAsync(string? search, string? sort, string? order, int? limit, int? offset);

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <returns>object</returns>
        public Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Post
        /// </summary>
        /// <returns>object</returns>
        public Task<T> PostAsync(T item);

        /// <summary>
        /// Put
        /// </summary>
        /// <returns>object</returns>
        public Task<T> PutAsync(T item);

        /// <summary>
        /// Delete
        /// </summary>
        /// <returns>boolean</returns>
        public Task<bool> DeleteAsync(Guid id);
    }
}
