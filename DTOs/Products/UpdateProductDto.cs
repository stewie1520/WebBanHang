using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBanHang.Models;

namespace WebBanHang.DTOs.Products
{
  public class UpdateProductDto : IValidatableObject
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int Price { get; set; }
    public bool IsVariant { get; set; } = false;
    public bool IsManageVariant { get; set; } = false;
    [Required]
    public int CategoryId { get; set; }
    public int? ParentId { get; set; }
    public List<string> ImageUrls { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Private;

    public List<UpdateProductDto> Children { get; set; }
    public List<string> Tags { get; set; }

    public List<string> Features { get; set; }
    public double Height { get; set; }
    public double Width { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }

    public bool IsDiscount { get; set; } = false;
    public int PriceBeforeDiscount { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (IsVariant && IsManageVariant)
      {
        yield return new ValidationResult(
            errorMessage: "Sản phẩm biến thể không được làm quản lý biến thể",
            memberNames: new[] { nameof(IsVariant), nameof(IsManageVariant) }
            );
      }
    }
  }
}
