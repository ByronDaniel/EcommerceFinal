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
    public class GenericRepository<T>: IGenericRepository<T> where T : CatalogueEntity
    {
        private readonly EcommerceDbContext context;

        public GenericRepository(EcommerceDbContext context)
        {
            this.context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().Where(t => t.Status).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            T item = await context.Set<T>().Where(t => t.Status && t.Id == id).SingleOrDefaultAsync();
            if (item == null)
                throw new ArgumentException($"No existe el registro con id: {id}");

            return item;
        }

        public async Task<T> PostAsync(T item)
        {
            bool itemExist = context.Set<T>().Any(t => t.Name == item.Name && t.Status);
            if (itemExist)
            {
                throw new ArgumentNullException($"Ya existe el registro con nombre: {item.Name}");
            }
            await context.Set<T>().AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<T> PutAsync(T item)
        {
            bool itemFind = context.Set<T>().Any(t=>t.Id == item.Id && t.Status);
            if (!itemFind)
                throw new ArgumentNullException($"No existe el registro con id: {item.Id}");
            
            itemFind = context.Set<T>().Any(t => t.Name.ToUpper() == item.Name.ToUpper() && t.Status);
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
            T item = await context.Set<T>().Where(t => t.Status && t.Id == id).SingleOrDefaultAsync();
            if (item == null)
                throw new ArgumentException($"No existe el registro con id: {id}");

            item.DateDeleted = DateTime.Now;
            item.Status = false;

            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
