using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.DTOs.Commons;
using WebBanHang.Models;

namespace WebBanHang.Services.WarehouseTransaction
{
  public interface IWarehouseTransactionService
  {
    Task<ServiceResponse<GetWarehouseTransactionDto>> CreateWarehouseTransactionAsync(CreateWarehouseTransactionDto dto);
    Task<ServiceResponse<GetWarehouseTransactionDto>> GetWarehouseTransactionAsync(int warehouseTransactionId);

    Task<ServiceResponse<IEnumerable<GetWarehouseTransactionWithoutItemDto>>> GetAllWarehouseTransactionsAsync(PaginationParam pagination, int type = 0);

    Task<ServiceResponse<GetWarehouseTransactionWithoutItemDto>> DeleteWarehouseTransactionAsync(int warehouseTransactionId);
    Task<ServiceResponse<List<GetManufacturerDto>>> GetAllManufacturersAsync();
    Task<ServiceResponse<List<GetProductDto>>> GetAllProductsAsync();
  }
}
