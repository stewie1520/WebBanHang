using System;

using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class WarehouseTransactionNotFoundException : BaseServiceException
    {
        public WarehouseTransactionNotFoundException() : base(ErrorCode.WAREHOUSE_TRANSACTION_NOT_FOUND, "warehouse transaction not found")
        {
        }
    }
}
