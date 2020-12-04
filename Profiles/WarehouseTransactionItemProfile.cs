using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using WebBanHang.Models;
using WebBanHang.DTOs.WarehouseTransactionItems;

namespace WebBanHang.Profiles
{
    public class WarehouseTransactionItemProfile : Profile
    {
        private class ProductIdFormatter : IValueConverter<Product, int>
        {
            public int Convert(Product product, ResolutionContext context)
            {
                return product.Id;
            }
        }
        public WarehouseTransactionItemProfile()
        {
            CreateMap<WarehouseTransactionItem, GetWarehouseTransactionItemDto>()
                .ForMember(dest => dest.ProductId, options => 
                    options.ConvertUsing(new ProductIdFormatter(), src => src.Product));
        }
    }
}
