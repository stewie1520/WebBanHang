using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebBanHang.DTOs.WarehouseTransactions
{
    using WebBanHang.Models;

    public class GetWarehouseTransactionDto
    {
        [Required]
        public WarehouseTransactionType TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public WarehouseTransactionStatus Status { get; set; }
        public int CreatedBy { get; set; }

        public IEnumerable<TransactionItem> Items { get; set; }

        public class TransactionItem
        {
            public string Name { get; set; }
            public IEnumerable<string> Images { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
