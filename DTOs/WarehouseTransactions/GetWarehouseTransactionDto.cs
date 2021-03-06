﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebBanHang.DTOs.User;
using WebBanHang.DTOs.WarehouseTransactionItems;
namespace WebBanHang.DTOs.WarehouseTransactions
{
  using WebBanHang.Models;

  public class GetWarehouseTransactionDto
  {
    public int Id { get; set; }
    [Required]
    public WarehouseTransactionType TransactionType { get; set; }
    public DateTime CreatedAt { get; set; }
    public WarehouseTransactionStatus Status { get; set; }
    public GetUserDto CreatedBy { get; set; }

    public string Description { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public IEnumerable<GetWarehouseTransactionItemDto> WarehouseTransactionItems { get; set; }

    public class TransactionItem
    {
      public string Name { get; set; }
      public IEnumerable<string> Images { get; set; }
      public int ProductId { get; set; }
      public int Quantity { get; set; }
    }
  }
}
