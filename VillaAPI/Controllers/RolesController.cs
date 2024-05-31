using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaAPI.DTOs.Role;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;

namespace VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IRoleRepository dbRole;
        internal APIResponse res;

        public RolesController(IRoleRepository _dbRole, IMapper _mapper)
        {
            dbRole = _dbRole;
            mapper = _mapper;
            res = new();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> AddRole(CreateRoleDTO createRole)
        {
                var roleModel = mapper.Map<IdentityRole>(createRole);

                var result = await dbRole.CreateOneRole(roleModel);

                if (!result.Succeeded) throw new AppError("Somethins Went Very wrong", HttpStatusCode.BadRequest);

                res.Result = new { role = createRole.Name };
                res.StatusCode = HttpStatusCode.Created;

                return Ok(res);
        }


        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllRoles()
        {

            var roles =await dbRole.GetAllRoles();


            res.Result = roles;
            res.StatusCode = HttpStatusCode.OK;

            return Ok(res);
        }


    }
}
