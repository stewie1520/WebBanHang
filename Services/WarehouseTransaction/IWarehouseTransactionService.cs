using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.DTOs.WarehouseTransactions;
using WebBanHang.Models;

namespace WebBanHang.Services.WarehouseTransaction
{
    public interface IWarehouseTransactionService
    {
        Task<ServiceResponse<GetWarehouseTransactionDto>> CreateWarehouseTransactionAsync(CreateWarehouseTransactionDto dto);
    }
}
