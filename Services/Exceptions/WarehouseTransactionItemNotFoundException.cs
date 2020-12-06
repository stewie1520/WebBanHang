using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebBanHang.Models;

namespace WebBanHang.Services.Exceptions
{
    public class WarehouseTransactionItemNotFoundException : BaseServiceException
    {
        public WarehouseTransactionItemNotFoundException() :
            base(ErrorCode.WAREHOUSE_TRANSACTION_ITEM_NOT_FOUND, "Could not found this item in warehouse transaction")
        {
        }
    }
}
