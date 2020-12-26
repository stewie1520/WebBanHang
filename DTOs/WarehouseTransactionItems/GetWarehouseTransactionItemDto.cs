using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.WarehouseTransactionItems
{
  public class GetWarehouseTransactionItemDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    [Required]
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Cost { get; set; }
  }
}
