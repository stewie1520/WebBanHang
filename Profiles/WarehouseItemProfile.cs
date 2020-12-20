using AutoMapper;
using WebBanHang.Models;
using WebBanHang.DTOs.WarehouseItems;


namespace WebBanHang.Profiles
{
  public class WarehouseItemProfile : Profile
  {
    public WarehouseItemProfile()
    {
      CreateMap<WarehouseItem, GetWarehouseItemDto>();
    }
  }
}
