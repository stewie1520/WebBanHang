using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class WarehouseItem : BaseModel, ISoftDelete, IValidatableObject
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double AverageCost { get; set; }
        // TODO: will fix later
        // public WarehouseImport LastImport { get; set; }
        // public WarehouseExport LastExport { get; set; }
        // public WarehouseAdjustment LastAdjustment { get; set; }
        public bool IsDeleted { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Quantity < 0)
            {
                yield return new ValidationResult
                (
                    errorMessage: "The quantity of product in warehouse is below 0",
                    memberNames: new[] { nameof(Quantity) }
                );
            }

            if (AverageCost < 0)
            {
                yield return new ValidationResult
                (
                    errorMessage: "The average cost of product in warehouse is below 0",
                    memberNames: new[] { nameof(AverageCost) }
                );
            }
        }
    }
}
