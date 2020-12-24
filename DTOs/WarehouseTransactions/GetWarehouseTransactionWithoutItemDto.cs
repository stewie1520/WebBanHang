using System;
using System.ComponentModel.DataAnnotations;

using WebBanHang.Models;

namespace WebBanHang.DTOs.WarehouseTransactions
{
  public class GetWarehouseTransactionWithoutItemDto
  {
    public int Id { get; set; }
    [Required]
    public WarehouseTransactionType TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
    public WarehouseTransactionStatus Status { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public int CreatedBy { get; set; }
    public string Description { get; set; }
  }
}
