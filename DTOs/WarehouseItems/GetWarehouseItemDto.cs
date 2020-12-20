using System.ComponentModel.DataAnnotations;

using WebBanHang.DTOs.Products;

namespace WebBanHang.DTOs.WarehouseItems
{
  public class GetWarehouseItemDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    public GetProductDto Product { get; set; }
    public int Quantity { get; set; }
    public double AverageCost { get; set; }
    // TODO: will fix later
    // public WarehouseImport LastImport { get; set; }
    // public WarehouseExport LastExport { get; set; }
    // public WarehouseAdjustment LastAdjustment { get; set; }
  }
}
