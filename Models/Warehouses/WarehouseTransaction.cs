using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
  public partial class WarehouseTransaction : BaseModel, ISoftDelete, IValidatableObject
  {
    [Required]
    public WarehouseTransactionType TransactionType { get; set; }

    public string Description { get; set; }
    public User CreatedBy { get; set; }
    public WarehouseTransactionStatus Status { get; set; } = WarehouseTransactionStatus.Processing;
    public IEnumerable<WarehouseTransactionItem> Items { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public bool IsDeleted { get; set; } = false;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (TransactionType == WarehouseTransactionType.Import && Manufacturer == null)
      {
        yield return new ValidationResult(
            errorMessage: "Bạn cần điền thông tin nhà cung cấp",
            memberNames: new[] { nameof(TransactionType), nameof(Manufacturer) }
        );
      }
    }
  }
}
