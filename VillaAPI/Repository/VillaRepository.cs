using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Repository
{
    public class VillaRepository:Repository<Villa> , IVillaRepository
    {
        private readonly AppDbContext context;

        public VillaRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }

        public new async Task UpdateOneAsync(Villa entity)
        {
            context.Villas.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
