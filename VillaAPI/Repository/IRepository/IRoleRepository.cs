using Microsoft.AspNetCore.Identity;

namespace VillaAPI.Repository.IRepository
{
    public interface IRoleRepository
    {
        public Task<IdentityResult> CreateOneRole(IdentityRole roleModel);
        public Task<List<IdentityRole>> GetAllRoles();
    }
}
