using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.GuestBaskets
{
  public class GuestBasketItemDto
  {

    [Required]
    public Product Product { get; set; }
    [Required]
    public int Price { get; set; }
    [Required]
    public int Quantity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (Price <= 0)
      {
        yield return new ValidationResult(
            "Price of product in order cannot be negative or zero",
            new[] { nameof(Price), nameof(BasketItem) });
      }

      if (Quantity <= 0)
      {
        yield return new ValidationResult(
            "Quantity of product in order cannot be negative or zero",
            new[] { nameof(Quantity), nameof(BasketItem) });
      }
    }
    [Required]
    public bool IsDeleted { get; set; } = false;
  }
}
