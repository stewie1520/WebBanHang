using System;
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
    }
}
