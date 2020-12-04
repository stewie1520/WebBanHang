using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.WarehouseTransactionItems
{
    public class GetWarehouseTransactionItemDto
    {
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
