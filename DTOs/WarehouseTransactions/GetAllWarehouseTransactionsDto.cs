using System;
using System.ComponentModel.DataAnnotations;

using WebBanHang.Models;

namespace WebBanHang.DTOs.WarehouseTransactions
{
    public class GetAllWarehouseTransactionsDto
    {
        public int Id { get; set; }
        [Required]
        public WarehouseTransactionType TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public WarehouseTransactionStatus Status { get; set; }
        public int CreatedBy { get; set; }
    }
}
