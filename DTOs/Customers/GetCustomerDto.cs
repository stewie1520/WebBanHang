using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

using WebBanHang.Models;

namespace WebBanHang.DTOs.Customers
{
  public class GetCustomerDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [EmailAddress, Required]
    public string Email { get; set; }
    [Required]
    public bool IsRegisted { get; set; }
    [Required]
    public Gender Gender { get; set; } = Gender.Unknown;
    public List<GetAddressDto> Addresses { get; set; }
  }
}
