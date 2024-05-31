using VillaAPI.Models;

namespace VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        public new Task UpdateOneAsync(VillaNumber entity);

    }
}
