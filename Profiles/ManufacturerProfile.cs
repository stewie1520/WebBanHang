using System;
using AutoMapper;

using WebBanHang.Models;
using WebBanHang.DTOs.Manufacturers;

namespace WebBanHang.Profiles
{
  public class ManufacturerProfile : Profile
  {
    public ManufacturerProfile()
    {
      CreateMap<CreateManufacturerDto, Manufacturer>();
      CreateMap<Manufacturer, GetManufacturerDto>();
    }
  }
}
