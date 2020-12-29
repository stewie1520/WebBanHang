using System.Collections.Generic;
using System.Linq;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Products
{
  public class QueryProductDto
  {
    public bool? SortByDate { get; set; }
    public bool? IsDiscount { get; set; }
    public int? ParentId { get; set; }
    public bool? IsVariant { get; set; }
    public bool? IsManageVariant { get; set; }
    public List<ProductStatus> Status { get; set; }
    public List<int> CategoryId { get; set; }
    public int Min { get; set; } = -1;
    public int Max { get; set; } = -1;
  }
}
