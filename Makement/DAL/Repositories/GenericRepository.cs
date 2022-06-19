using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DatabseContext;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public abstract class GenericRepository<T, Tkey> : IGenericRepository<T, Tkey> where T : class
    {
        protected readonly DatabaseContext context;

        protected GenericRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public virtual async Task Add(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public virtual async Task<T> Get(Tkey id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
