namespace WebBanHang.DTOs.WarehouseTransactions
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using WebBanHang.Models;
  using WebBanHang.DTOs.WarehouseTransactionItems;
  using System.Collections.Generic;

  public class CreateWarehouseTransactionDto : IValidatableObject
  {
    [Required]
    public WarehouseTransactionType TransactionType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int ManufacturerId { get; set; }

    public string Description { get; set; }

    public IEnumerable<AddWarehouseItemDto> WarehouseTransactionItems { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (TransactionType == WarehouseTransactionType.Import && ManufacturerId == 0)
      {
        yield return new ValidationResult(
            errorMessage: "Bạn cần điền thông tin nhà cung cấp",
            memberNames: new[] { nameof(TransactionType), nameof(ManufacturerId) }
        );
      }
    }
  }
}
