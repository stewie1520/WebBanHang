using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Models
{
    public partial class WarehouseTransaction
    {
        public bool CanModify() => this.Status == WarehouseTransactionStatus.Processing;
    }
}
