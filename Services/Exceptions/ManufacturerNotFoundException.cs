using System;
using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
  public class ManufacturerNotFoundException : BaseServiceException
  {
    public ManufacturerNotFoundException() : base(ErrorCode.MANUFACTURER_NOT_FOUND_ERROR, "Không tìm thấy nhà sản xuất")
    {
    }
  }
}
