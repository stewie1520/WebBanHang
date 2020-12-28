using WebBanHang.DTOs.Baskets;
using WebBanHang.DTOs.BasketItems;
using AutoMapper;
using WebBanHang.Models;
using System.Linq;

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
            CreateMap<UpdateBasketStatusDto, Basket>();
            CreateMap<BasketItem, GetBasketItemDto>()
                 .ForMember(destination => destination.ProductId, options => options.ConvertUsing(new ProductInBasketFormatter(), src => src.Product))
                 .ForMember(destination => destination.Avatar, options => options.ConvertUsing(new AvatarFormatter(), src => src.Product))
                 .ForMember(destination => destination.Name, options => options.MapFrom(src => src.Product.Name));
            CreateMap<Basket, GetBasketDto>();
            CreateMap<Basket, GetBasketWithoutItemDto>();
           
            // Create basket Items
            CreateMap<CreateBasketItemDto, BasketItem>();
        
        }
        private class AvatarFormatter : IValueConverter<Product, string>
        {
            public string Convert(Product sourceMember, ResolutionContext context)
            {
                if (sourceMember == null) return "";

                return sourceMember.Images?.FirstOrDefault()?.Url ?? "KHONGHINH";
            }
        }
        private class ProductInBasketFormatter : IValueConverter<Product, int>
        {
            public int Convert(Product sourceMember, ResolutionContext context)
            {
                if (sourceMember == null) return 0;

                return sourceMember.Id;
            }
        }
    }
}