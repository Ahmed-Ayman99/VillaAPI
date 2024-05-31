using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System.Linq.Expressions;

namespace VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, bool tracked = true, string include = null);
        public Task<List<T>> GetAllAsync(string? include = null, int page = 1, int pageSize = 10);
        public Task CreateOneAsync(T entity);
        public Task UpdateOneAsync(T entity);
        public Task DeleteOneAsync(T entity);
        public Task SaveAsync();
    }
}
