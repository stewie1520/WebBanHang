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
                .ForMember(dto => dto.ImageUrls, opts => opts.ConvertUsing(new ProductImageFormatter(), src => src.Images));
        }

        private class ProductImageFormatter : IValueConverter<IEnumerable<ProductImage>, IEnumerable<string>>
        {
            public IEnumerable<string> Convert(IEnumerable<ProductImage> images, ResolutionContext context)
            {
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
