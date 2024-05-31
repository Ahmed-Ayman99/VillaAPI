using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;
using VillaAPI.utils;
namespace VillaAPI.Repository
{
    public class UserAccountRepository:IUserAccountRepository
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public UserAccountRepository(IConfiguration _configuration , UserManager<ApplicationUser> _userManager)
        {
            configuration = _configuration;
            userManager = _userManager;
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user , string password) {
            var userResult = await userManager.CreateAsync(user, password);
            return userResult;
        }

        public async Task AddRoleToUser(ApplicationUser user)
        {
            await userManager.AddToRoleAsync(user, "User");
        }

        public JwtSecurityToken CreateToken(List<Claim> claims) {

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:SecretKey")));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create Token
            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: signingCredentials
                );


        

            return token;
        }

        public async Task<JwtSecurityToken> Login(string email ,string password) {
            var userModel = await userManager.FindByEmailAsync(email);

            if (userModel == null) throw new AppError("Email is not existed" , HttpStatusCode.Unauthorized);
                
            var correctPass = await userManager.CheckPasswordAsync(userModel, password);

            if (!correctPass) throw new AppError("Password is not correct", HttpStatusCode.Unauthorized);

            
            List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,userModel.UserName),
                        new Claim(ClaimTypes.NameIdentifier,userModel.Id),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            
            var roles = await userManager.GetRolesAsync(userModel);

            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            var token = CreateToken(claims);

            return token;
                
        }

        public async Task<ApplicationUser> GetUserById(string name) {
            var userModel = await userManager.FindByNameAsync(name);
            
            if(userManager == null) throw new AppError("User With Id doesn't existed", HttpStatusCode.NotFound);

            return userModel!;
        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            var userModel = await userManager.Users.ToListAsync();
            
            return userModel!;
        }
    }
}
