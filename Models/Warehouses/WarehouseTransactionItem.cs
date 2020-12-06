using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class WarehouseTransactionItem : BaseModel, ISoftDelete, IValidatableObject
    {
        [Required]
        public WarehouseTransaction WarehouseTransaction { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }

        public bool IsDeleted { get; set; } = false;

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (Quantity <= 0)
            {
                yield return new ValidationResult
                (
                    errorMessage: $"Quantity of item with id {Product.Id} below 0",
                    memberNames: new[] { nameof(Product), nameof(Quantity) }
                );
            }
        }
    }
}
