using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.Products;

namespace WebBanHang.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, GetProductDto>()
                .ForMember(dto => dto.ParentId, opts => opts.ConvertUsing(new ParentIdFormatter(), src => src.Parent))
                .ForMember(dto => dto.ImageUrls, opts => opts.ConvertUsing(new ProductImageFormatter(), src => src.Images))
                .ForMember(dto => dto.CategoryId, opts => opts.ConvertUsing(new CategoryFormater(), src => src.Category));
        }

        private class CategoryFormater : IValueConverter<Category, int>
        {
            public int Convert(Category category, ResolutionContext context)
            {
                return category.Id;
            }
        }

        private class ProductImageFormatter : IValueConverter<IEnumerable<ProductImage>, IEnumerable<string>>
        {
            public IEnumerable<string> Convert(IEnumerable<ProductImage> images, ResolutionContext context)
            {
                if (images == null) return new List<string>();
                return images.Select(image => image.Url).ToList();
            }
        }

        private class ParentIdFormatter : IValueConverter<Product, int?>
        {
            public int? Convert(Product product, ResolutionContext context)
            {
                if (product == null) return null;
                return product.Id;
            }
        }
    }
}
