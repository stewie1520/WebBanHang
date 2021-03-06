using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
  public class Product : BaseModel, ISoftDelete
  {
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int Price { get; set; }
    public bool IsVariant { get; set; } = false;
    public bool IsManageVariant { get; set; } = false;
    [Required]
    public Category Category { get; set; }
    public Product Parent { get; set; }
    public List<Product> Children { get; set; }
    public IEnumerable<ProductImage> Images { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Private;
    [Required]
    public bool IsDeleted { get; set; } = false;
    public WarehouseItem WarehouseItem { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Features { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }

    public bool IsDiscount { get; set; } = false;
    public int PriceBeforeDiscount { get; set; }
  }
}
