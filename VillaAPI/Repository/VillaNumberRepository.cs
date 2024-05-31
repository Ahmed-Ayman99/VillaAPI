using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly AppDbContext context;

        public VillaNumberRepository(AppDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
