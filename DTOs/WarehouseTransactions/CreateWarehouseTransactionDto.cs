namespace WebBanHang.DTOs.WarehouseTransactions
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using WebBanHang.Models;
  using System.Collections.Generic;

  public class CreateWarehouseTransactionDto : IValidatableObject
  {
    [Required]
    public WarehouseTransactionType TransactionType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Manufacturer Manufacturer { get; set; }

    public string Description { get; set; }
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
