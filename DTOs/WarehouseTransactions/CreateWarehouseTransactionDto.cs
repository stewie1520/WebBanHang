namespace WebBanHang.DTOs.WarehouseTransactions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using WebBanHang.Models;
    using System.Collections.Generic;

    public class CreateWarehouseTransactionDto
    {
        [Required]
        public WarehouseTransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
