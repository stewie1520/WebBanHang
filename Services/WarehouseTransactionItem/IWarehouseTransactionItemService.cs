using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebBanHang.Models;
using WebBanHang.DTOs.WarehouseTransactionItems;

namespace WebBanHang.Services.WarehouseTransactionItem
{
    public interface IWarehouseTransactionItemService
    {
        Task<ServiceResponse<GetWarehouseTransactionItemDto>> AddItemToTransactionAsync(int warehouseTransactionId, AddWarehouseItemDto dto);

        Task<ServiceResponse<RemoveWarehouseItemDto>> RemoveItemFromTransactionAsync(int warehouseTransactionId, RemoveWarehouseItemDto dto);
        Task<ServiceResponse<GetWarehouseTransactionItemDto>> AddItemToTransactionAsync(int warehouseTransactionId, IEnumerable<AddWarehouseItemDto> dtos);
    }
}
