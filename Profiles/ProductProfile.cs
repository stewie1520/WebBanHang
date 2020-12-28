using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.Products;
using WebBanHang.DTOs.Categories;

namespace WebBanHang.Profiles
{
  public class ProductProfile : Profile
  {
    public ProductProfile()
    {
      CreateMap<CreateProductDto, Product>();
      CreateMap<UpdateProductDto, Product>();
      CreateMap<Product, GetAllProductShopDto>()
        .ForMember(dto => dto.Title, opts => opts.MapFrom(p => p.Name))
        .ForMember(dto => dto.Price, opts => opts.MapFrom(p => p.Price))
        .ForMember(dto => dto.Sale, opts => opts.MapFrom(p => p.IsDiscount))
        .ForMember(dto => dto.SalePrice, opts => opts.MapFrom(p => p.PriceBeforeDiscount))
        .ForMember(dto => dto.Thumbnail, opts => opts.ConvertUsing(new ThumbnailConverter(), p => p.Images))
        .ForMember(dto => dto.Variants, opts => opts.ConvertUsing(new ThumbnailVariantsConverter(), p => p.Images))
        .ForMember(dto => dto.Categories, opts => opts.ConvertUsing(new CategoriesShopConverter(), p => p.Category))
        ;
      CreateMap<Product, GetProductDto>()
          .ForMember(dto => dto.ParentId, opts => opts.ConvertUsing(new ParentIdFormatter(), src => src.Parent))
          .ForMember(dto => dto.ImageUrls, opts => opts.ConvertUsing(new ProductImageFormatter(), src => src.Images))
          .ForMember(dto => dto.CategoryId, opts => opts.ConvertUsing(new CategoryFormater<int>(), src => src.Category))
          .ForMember(dto => dto.CategoryText, opts => opts.ConvertUsing(new CategoryFormater<string>(), src => src.Category))
          .ForMember(dto => dto.Children, opts => opts.ConvertUsing(new ChildrenFormatter(), src => src.Children));
    }

    private class ChildrenFormatter : IValueConverter<IEnumerable<Product>, IEnumerable<GetProductDto>>
    {
      public IEnumerable<GetProductDto> Convert(IEnumerable<Product> products, ResolutionContext context)
      {
        var result = new List<GetProductDto>();

        if (products == null)
        {
          return result;
        }

        foreach (var product in products)
        {
          if (product != null)
          {
            result.Add(context.Mapper.Map<GetProductDto>(product));
          }
        }

        return result;
      }
    }

    private class CategoriesShopConverter : IValueConverter<Category, List<GetCategoryShopDto>>
    {
      public List<GetCategoryShopDto> Convert(Category sourceMember, ResolutionContext context)
      {
        if (sourceMember == null) return null;

        return new List<GetCategoryShopDto>
        {
          new GetCategoryShopDto { Id = sourceMember.Id, Value = $"{sourceMember.Id}", Name = sourceMember.Name }
        };
      }
    }

    private class CategoryFormater<T> : IValueConverter<Category, T>
    {
      public T Convert(Category category, ResolutionContext context)
      {
        if (typeof(T).Name == typeof(int).Name)
        {
          return (T)System.Convert.ChangeType(category.Id, typeof(T));
        }

        return (T)System.Convert.ChangeType(category.Name, typeof(T));
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

    private class ThumbnailConverter : IValueConverter<IEnumerable<ProductImage>, string>
    {
      public string Convert(IEnumerable<ProductImage> sourceMember, ResolutionContext context)
      {
        if (sourceMember == null) return "";
        return sourceMember.FirstOrDefault()?.Url ?? "";
      }
    }

    private class ThumbnailVariantsConverter : IValueConverter<IEnumerable<ProductImage>, IEnumerable<GetAllProductShopDto.ThumbnailVariant>>
    {
      public IEnumerable<GetAllProductShopDto.ThumbnailVariant> Convert(IEnumerable<ProductImage> sourceMember, ResolutionContext context)
      {
        if (sourceMember == null) return null;
        int id = 1;
        var result = new List<GetAllProductShopDto.ThumbnailVariant>();

        foreach (var productImage in sourceMember)
        {
          result.Add(new GetAllProductShopDto.ThumbnailVariant { Id = id, Thumbnail = productImage.Url });
        }

        return result;
      }
    }
  }
}
