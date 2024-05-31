using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using VillaAPI.DTOs.User;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;

namespace VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountRepository dbUser;
        private readonly IMapper mapping;
        internal APIResponse res;

        public AccountController(IUserAccountRepository _dbUser ,  IMapper _mapping)
        {
            dbUser = _dbUser;
            mapping = _mapping;
            res = new();
        }

        [HttpPost("register")]
        public async Task<ActionResult<APIResponse>> Register(RegisterUserDTO registerUserDTO)
        {
            var user = mapping.Map<ApplicationUser>(registerUserDTO);

            var result = await dbUser.CreateUser(user, registerUserDTO.Password);

            if (!result.Succeeded) return BadRequest(new { message = "Something went Wrong Please Try Try Again", errors = result.Errors });

            await dbUser.AddRoleToUser(user);

            List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(ClaimTypes.Role,"User"),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };

            var token = dbUser.CreateToken(claims);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            res.StatusCode = HttpStatusCode.Created;
            res.Result = new
            {
                Expiration = token.ValidTo,
                Token = tokenString,
                User = user
            };

            return CreatedAtRoute("GetUser", new { name = user.UserName}, res);

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUserDTO) {
            var token = await dbUser.Login(loginUserDTO.Email , loginUserDTO.Password);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            res.StatusCode = HttpStatusCode.Created;
            res.Result = new
            {
                Expiration = token.ValidTo,
                Token = tokenString,
            };

            return Ok(res);

        }

        [Authorize]
        [HttpGet("{name:alpha}", Name = "GetUser")]
        public async Task<IActionResult> GetOneUser(string name) {
            ApplicationUser user = await dbUser.GetUserById(name);

            res.Result = user;
            res.StatusCode = HttpStatusCode.OK;

            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await dbUser.GetAllUser();

            res.Result = users;
            res.StatusCode = HttpStatusCode.OK;

            return Ok(res);
        }
    }
}
