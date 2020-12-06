using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using WebBanHang.DTOs.WarehouseTransactionItems;

namespace WebBanHang.DTOs.WarehouseTransactionItems
{
    public class RemoveWarehouseItemDto : IValidatableObject
    {
        public int? ProductId { get; set; }
        public int? WarehouseTransactionItemId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ProductId.HasValue && !WarehouseTransactionItemId.HasValue)
            {
                yield return new ValidationResult(
                    errorMessage: "Required at least ProductId or WarehouseTransactionItemId",
                    memberNames: new[] { nameof(ProductId), nameof(WarehouseTransactionItemId) }
                );
            }
        }
    }
}
