using BP.Ecommerce.Domain.Entities;
using BP.Ecommerce.Domain.RepositoriesInterfaces;
using BP.Ecommerce.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BP.Ecommerce.Infraestructure.RepositoriesImplementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : CatalogueEntity
    {
        private readonly EcommerceDbContext context;

        public GenericRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public async Task<List<T>> GetAllAsync(string? search, string? sort, string? order, int? limit, int? offset)
        {
            //filter by status
            var query = context.Set<T>().Where(t => t.State == Status.Vigente.ToString());
            //filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Name == search);
            }
            //Sort and order
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToUpper())
                {
                    case "NAME":
                        query = query.OrderBy(t => t.Name);
                        break;

                }
            }

            query = query.OrderBy(t => t.Name);
            //pagination

            return await context.Set<T>().Where(t => t.State == Status.Vigente.ToString()).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            T item = await context.Set<T>().Where(t => t.State == Status.Vigente.ToString() && t.Id == id).SingleOrDefaultAsync();
            return item;
        }

        public async Task<T> PostAsync(T item)
        {
            bool itemExist = context.Set<T>().Any(t => t.Name == item.Name && t.State == Status.Vigente.ToString());
            if (itemExist)
            {
                throw new ArgumentException($"Ya existe el registro con nombre: {item.Name}");
            }
            item.Name = item.Name.ToUpper();
            await context.Set<T>().AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<T> PutAsync(T item)
        {
            bool itemFind = context.Set<T>().Any(t => t.Id == item.Id && t.State == Status.Vigente.ToString());
            if (!itemFind)
                throw new ArgumentException($"No existe el registro con id: {item.Id}");

            itemFind = context.Set<T>().Any(t => t.Name.ToUpper() == item.Name.ToUpper() && t.State == Status.Vigente.ToString() && t.Id != item.Id);
            if (itemFind)
            {
                throw new ArgumentException($"Ya existe el registro con nombre: {item.Name}");
            }

            item.DateModification = DateTime.Now;
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            T item = await context.Set<T>().Where(t => t.State == Status.Vigente.ToString() && t.Id == id).SingleOrDefaultAsync();
            if (item == null)
                throw new ArgumentException($"No existe el registro con id: {id}");

            item.DateDeleted = DateTime.Now;
            item.State = Status.Eliminado.ToString();

            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

    }
}
