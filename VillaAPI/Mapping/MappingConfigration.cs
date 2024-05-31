using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VillaAPI.DTOs.Role;
using VillaAPI.DTOs.User;
using VillaAPI.DTOs.Villa;
using VillaAPI.DTOs.VillaNumber;
using VillaAPI.Models;

namespace VillaAPI.Mapping
{
    public class MappingConfigration : Profile
    {
        public MappingConfigration() 
        {
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaDTO>().ReverseMap();


            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();

            CreateMap<IdentityRole, CreateRoleDTO>().ReverseMap();
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();



            CreateMap<ApplicationUser, RegisterUserDTO>().ReverseMap();
        }
    }
}
