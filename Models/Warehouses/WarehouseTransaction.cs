using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class WarehouseTransaction : BaseModel, ISoftDelete
    {
        [Required]
        public WarehouseTransactionType TransactionType { get; set; }
        public User CreatedBy { get; set; }
        public WarehouseTransactionStatus Status { get; set; } = WarehouseTransactionStatus.Processing;
        public IEnumerable<WarehouseTransactionItem> Items { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
