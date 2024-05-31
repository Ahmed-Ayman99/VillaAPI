using System.Linq.Expressions;
using VillaAPI.Models;

namespace VillaAPI.Repository.IRepository
{
    public interface IVillaRepository:IRepository<Villa>
    {
        public new Task UpdateOneAsync(Villa entity);
    }
}
