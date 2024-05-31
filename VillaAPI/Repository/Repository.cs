using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using VillaAPI.Data;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;

namespace VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext context;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext _context)
        {
            context = _context;
            dbSet = context.Set<T>();   
        }

        public async Task<List<T>> GetAllAsync(string? include, int page, int pageSize)
        {
            var skip = (page-1) * pageSize;
            var query = dbSet.AsQueryable();

            if (pageSize > 100) pageSize = 100;

            query.Skip(skip).Take(pageSize);

            if (!string.IsNullOrEmpty(include) ) query = query.Include(include);
            
            return await query.ToListAsync();
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, bool tracked, string include = null)
        {
            var query= dbSet.AsQueryable();

            if(!tracked) query = query.AsNoTracking();
            if(include!= null ) query = query.Include(include);

            return await query.FirstOrDefaultAsync(filter);
        }


        public async Task CreateOneAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        
        }
        public async Task UpdateOneAsync(T entity)
        {
            dbSet.Update(entity);
            await SaveAsync();

        }

        public async Task DeleteOneAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}
