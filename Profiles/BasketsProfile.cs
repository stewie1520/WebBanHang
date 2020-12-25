using WebBanHang.DTOs.Baskets;
using AutoMapper;
using WebBanHang.Models;

namespace WebBanHang.Profiles
{
    public class BasketProfile : Profile
    {
        // class CustomerIdFormater : IValueConverter<Basket, int?>
        // {
        //     public int? Convert(Basket sourceMember, ResolutionContext context)
        //     {
        //         if (sourceMember == null) return null;
        //         return sourceMember.Id;
        //     }
        // }
        public BasketProfile(){
            CreateMap<CreateBasketDto, Basket>();
            CreateMap<Basket, GetBasketDto>();
        }
    }
}