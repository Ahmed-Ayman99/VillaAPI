using System.ComponentModel.DataAnnotations;

namespace VillaAPI.DTOs.User
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
