using WebBanHang.Models;

namespace WebBanHang.DTOs.Products
{
  public class QueryProductDto
  {
    public int? ParentId { get; set; }
    public bool? IsVariant { get; set; }
    public bool? IsManageVariant { get; set; }
    public ProductStatus? Status { get; set; }
    public int? CategoryId { get; set; }
  }
}
