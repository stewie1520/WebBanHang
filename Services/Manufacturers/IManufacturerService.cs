using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebBanHang.Models;
using WebBanHang.DTOs.Manufacturers;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.Manufacturers
{
  public interface IManufacturerService
  {
    Task<ServiceResponse<GetManufacturerDto>> CreateManufacturerAsync(CreateManufacturerDto dto);
    Task<ServiceResponse<GetManufacturerDto>> UpdateManufacturerAsync(UpdateManufacturerDto dto);
    Task<ServiceResponse<GetManufacturerDto>> GetManufacturerAsync(int manufacturerId);
    Task<ServiceResponse<List<GetManufacturerDto>>> GetAllManufacturerAsync(PaginationParam pagination, QueryManufacturerDto query);
    Task<ServiceResponse<int>> DeleteManufacturerAsync(int manufacturerId);
  }
}
