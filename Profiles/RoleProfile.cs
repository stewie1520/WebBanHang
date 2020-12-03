using System;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.Roles;
using System.Linq;
namespace WebBanHang.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, GetRoleDto>();
        }
    }
}
