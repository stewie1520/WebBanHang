using System;
using AutoMapper;

using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.Models;

namespace WebBanHang.Profiles
{
    public class WarehouseTransactionProfile : Profile
    {
        private class CreatedByFormatter : IValueConverter<User, int>
        {
            public int Convert(User user, ResolutionContext context)
            {
                return user.Id;
            }
        }

        public WarehouseTransactionProfile()
        {
            CreateMap<CreateWarehouseTransactionDto, WarehouseTransaction>();
            CreateMap<WarehouseTransaction, GetWarehouseTransactionDto>()
                .ForMember(dest => dest.CreatedBy, option => option.ConvertUsing(new CreatedByFormatter(), src => src.CreatedBy));
        }
    }
}
