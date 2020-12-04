using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class WarehouseTransactionModifiedException : BaseServiceException
    {
        public WarehouseTransactionModifiedException() : base(ErrorCode.WAREHOUSE_TRANSACTION_NO_ALLOWING_MODIFIED,
            "Warehouse transaction is not allowed modifying")
        {
        }
    }
}
