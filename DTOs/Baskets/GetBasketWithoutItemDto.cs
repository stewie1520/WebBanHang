using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

using WebBanHang.DTOs.BasketItems;
using WebBanHang.Models;
using WebBanHang.DTOs.Customers;

namespace WebBanHang.DTOs.Baskets
{
  public class GetBasketWithoutItemDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public bool IsPaid { get; set; }
    public BasketStatus Status { get; set; } = BasketStatus.Ordering;
    [Required]
    public GetCustomerDto Customer { get; set; }
    [Required]
    public string Note { get; set; }
    [Required]
    public int TotalPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
