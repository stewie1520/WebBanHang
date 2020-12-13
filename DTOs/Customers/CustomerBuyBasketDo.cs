using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.Customers
{
  public class CustomerBuyBasketDto
  {
    [Required]
    public string FullName { get; set; }
    [EmailAddress]
    public string Email { get; set; } = "noemail@guest.com";
    [Required]
    public CustomerAddressDto Address { get; set; }
  }
}
