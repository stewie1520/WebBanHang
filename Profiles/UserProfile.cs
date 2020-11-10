using System;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.User;

namespace WebBanHang.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterDto>()
                .ForMember(x => x.Password, opt => opt.Ignore());
            CreateMap<UserRegisterDto, User>()
                .ForMember(x => x.Password, opt => opt.Ignore());
        }
    }
}
