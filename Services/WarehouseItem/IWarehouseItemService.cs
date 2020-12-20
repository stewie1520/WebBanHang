using System.Threading.Tasks;
using System.Collections.Generic;

using WebBanHang.Models;
using WebBanHang.DTOs.WarehouseItems;
using WebBanHang.DTOs.Commons;

namespace WebBanHang.Services.WarehouseItem
{
  public interface IWarehouseItemService
  {
    Task<ServiceResponse<List<GetWarehouseItemDto>>> GetAllWarehouseItemsAsync(PaginationParam pagination, QueryWarehouseItemDto query);
  }
}
