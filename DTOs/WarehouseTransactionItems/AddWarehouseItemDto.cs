using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.WarehouseTransactionItems
{
  public class AddWarehouseItemDto : IValidatableObject
  {
    [Required]
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public double Cost { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (Quantity <= 0)
      {
        yield return new ValidationResult(
            errorMessage: "Quantity is below 0",
            memberNames: new[] { nameof(Quantity) }
            );
      }

      if (Cost < 0)
      {
        yield return new ValidationResult(
            errorMessage: "Cost is below 0",
            memberNames: new[] { nameof(Cost) }
            );
      }
    }
  }
}
