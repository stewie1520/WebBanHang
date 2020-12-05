using System;
using System.Linq;
using System.Collections.Generic;
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
                .ForMember(dest => dest.CreatedBy, option => option.ConvertUsing(new CreatedByFormatter(), src => src.CreatedBy))
                .ForMember(dest => dest.Items, option => option.ConvertUsing(new ItemsFormatter(), src => src.Items));

            CreateMap<WarehouseTransaction, GetAllWarehouseTransactionsDto>();
        }
    }
}
