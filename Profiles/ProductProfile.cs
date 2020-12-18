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
      CreateMap<UpdateProductDto, Product>();
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
  }
}
