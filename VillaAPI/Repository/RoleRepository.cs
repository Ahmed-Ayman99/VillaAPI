using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleRepository(IMapper _mapper, RoleManager<IdentityRole> _roleManager)
        {
            mapper = _mapper;
            roleManager = _roleManager;
        }

        public async Task<IdentityResult> CreateOneRole(IdentityRole roleModel)
        {
            return await roleManager.CreateAsync(roleModel);
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var roles =  await roleManager.Roles.ToListAsync();

            return roles;
        }
    }
}
