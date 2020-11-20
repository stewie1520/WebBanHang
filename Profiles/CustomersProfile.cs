using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;
using WebBanHang.DTOs.Customers;
namespace WebBanHang.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<CustomerRegisterDto, Customer>()
                .ForMember(x => x.Password, opt => opt.Ignore());
            CreateMap<Customer, CustomerRegisterDto>()
                .ForMember(x => x.Password, opt => opt.Ignore());
        }
    }
}
