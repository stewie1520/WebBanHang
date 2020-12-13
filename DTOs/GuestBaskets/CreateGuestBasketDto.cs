using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using WebBanHang.Models;
using WebBanHang.DTOs.Customers;

namespace WebBanHang.DTOs.GuestBaskets
{
  public class CreateGuestBasketDto
  {
    public CustomerBuyBasketDto Customer { get; set; }
    public string Note { get; set; }
    public IEnumerable<GuestB>
  }
}
