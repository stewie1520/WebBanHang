﻿using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;

using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.Models;

namespace WebBanHang.Profiles
{
  public class WarehouseTransactionProfile : Profile
  {
    private class CostFormatter : IValueConverter<WarehouseItem, double>
    {
      public double Convert(WarehouseItem sourceMember, ResolutionContext context)
      {
        if (sourceMember != null)
        {
          return sourceMember.AverageCost;
        }
        return 0;
      }
    }

    private class AvatarFormatter : IValueConverter<IEnumerable<ProductImage>, string>
    {
      public string Convert(IEnumerable<ProductImage> sourceMember, ResolutionContext context)
      {
        if (sourceMember.Count() == 0)
          return "";
        return sourceMember.First()?.Url ?? "";
      }
    }
    private class CreatedByFormatter : IValueConverter<User, int>
    {
      public int Convert(User user, ResolutionContext context)
      {
        return user.Id;
      }
    }

    private class ItemsFormatter : IValueConverter<IEnumerable<WarehouseTransactionItem>, IEnumerable<GetWarehouseTransactionDto.TransactionItem>>
    {
      public IEnumerable<GetWarehouseTransactionDto.TransactionItem> Convert(
          IEnumerable<WarehouseTransactionItem> src, ResolutionContext context)
      {
        if (src == null)
        {
          return null;
        }

        var result = (from item in src
                      select new GetWarehouseTransactionDto.TransactionItem()
                      {
                        Images = item.Product.Images.Select(img => img.Url).ToList(),
                        Name = item.Product.Name,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity
                      }).ToList();

        return result;
      }
    }

    public WarehouseTransactionProfile()
    {
      CreateMap<CreateWarehouseTransactionDto, WarehouseTransaction>();
      CreateMap<WarehouseTransaction, GetWarehouseTransactionDto>()
          // .ForMember(dest => dest.CreatedBy, option => option.ConvertUsing(new CreatedByFormatter(), src => src.CreatedBy))
          .ForMember(dest => dest.Items, option => option.ConvertUsing(new ItemsFormatter(), src => src.Items));

      CreateMap<WarehouseTransaction, GetWarehouseTransactionWithoutItemDto>();
      CreateMap<Manufacturer, GetManufacturerDto>();
      CreateMap<Product, GetProductDto>()
        .ForMember(dest => dest.Avatar, option => option.ConvertUsing(new AvatarFormatter(), src => src.Images))
        .ForMember(dest => dest.Cost, option => option.ConvertUsing(new CostFormatter(), src => src.WarehouseItem));
    }
  }
}
