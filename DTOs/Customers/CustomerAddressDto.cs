using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.DTOs.Customers
{
  public class CustomerAddressDto
  {
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Street { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string District { get; set; }
    [Required]
    public string Ward { get; set; }
  }
}
