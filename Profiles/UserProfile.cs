using System;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.User;
using System.Linq;
namespace WebBanHang.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDto>()
                .ForMember(des => des.Roles, c => c.MapFrom(c => c.UserRoles.Select(ur => ur.Role)));
            CreateMap<CreateUserDto, User>()
                .ForMember(x => x.Password, opt => opt.Ignore());
        }
    }
}
