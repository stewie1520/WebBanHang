using System;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.Categories;

namespace WebBanHang.Profiles
{
    public class CategoriesProfile : Profile
    {
        class ParentIdFormatter : IValueConverter<Category, int?>
        {
            public int? Convert(Category sourceMember, ResolutionContext context)
            {
                if (sourceMember == null) return null;
                return sourceMember.Id;
            }
        }

        public CategoriesProfile()
        {
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, GetCategoryDto>()
                .ForMember(c => c.ParentId, opt => opt.ConvertUsing(new ParentIdFormatter(), src => src.Parent));

            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
